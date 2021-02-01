import glob
import logging
import os
import os.path
import shutil
import socket
import subprocess
import sys
import tempfile
import time
import traceback

import archive
import backend
import compiler
import util

TEMP_DIR = os.getcwd()

BOMBERJAM_EXEC_NAME = 'bomberjam'

COMPILE_ERROR_MESSAGE = """
Your bot caused unexpected behavior in our servers.
For our reference, here is the trace of the error:
"""

UPLOAD_ERROR_MESSAGE = """
We had some trouble uploading your bot.
For our reference, here is the trace of the error:
"""

BOT_COMMAND = "cgexec -g cpu,memory,devices:{cgroup} sudo -Hiu {bot_user} bash -c 'cd \"{bot_dir}\" && ./{runfile}'"


def handle_compile_task(user_id):
    """Downloads and compiles a bot, then posts the compiled bot archive back through the API"""
    errors = []

    with tempfile.TemporaryDirectory(dir=TEMP_DIR) as temp_dir:
        try:
            bot_path = backend.download_bot(user_id, temp_dir, is_compiled=False)
            archive.unpack(bot_path)

            # Make sure things are in the top-level directory
            while len([
                name for name in os.listdir(temp_dir)
                if os.path.isfile(os.path.join(temp_dir, name))
            ]) == 0 and len(glob.glob(os.path.join(temp_dir, "*"))) == 1:
                with tempfile.TemporaryDirectory(dir=TEMP_DIR) as buffer_folder:
                    single_folder = glob.glob(os.path.join(temp_dir, "*"))[0]

                    for filename in os.listdir(single_folder):
                        shutil.move(os.path.join(single_folder, filename), buffer_folder)

                    os.rmdir(single_folder)

                    for filename in os.listdir(buffer_folder):
                        shutil.move(os.path.join(buffer_folder, filename), temp_dir)

            # Delete any symlinks
            subprocess.call(["find", temp_dir, "-type", "l", "-delete"])

            # Give the compilation user access
            os.chmod(temp_dir, 0o755)

            # User needs to be able to write to the directory and create files
            util.give_ownership(temp_dir, "bots", 0o2770)

            # Reset cwd before compilation, in case it was in a deleted temporary folder
            os.chdir(os.path.dirname(os.path.realpath(sys.argv[0])))
            language, more_errors = compiler.compile_anything(temp_dir)
            did_compile = more_errors is None
            if more_errors:
                errors.extend(more_errors)
        except Exception as ex:
            language = "Other"
            errors = [COMPILE_ERROR_MESSAGE + traceback.format_exc(), str(ex)] + errors
            did_compile = False

        try:
            if did_compile:
                logging.debug('Bot did compile')

                # Make things group-readable
                subprocess.call([
                    "sudo", "-H", "-u", "bot_compilation", "-s", "chmod", "-R", "g+r", temp_dir
                ], stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)

                archive_path = os.path.join(temp_dir, str(user_id) + ".zip")
                archive.pack(temp_dir, archive_path)
                backend.upload_bot(user_id, archive_path)
            else:
                logging.debug("Bot did not compile")
                logging.debug("Bot errors: %s" % str(errors))

            backend.send_compilation_result(user_id, did_compile, language, errors="\n".join(errors))

        except Exception as ex:
            logging.error("Bot did not upload", ex)
            errors.append(UPLOAD_ERROR_MESSAGE + traceback.format_exc())
            backend.send_compilation_result(user_id, False, language, errors="\n".join(errors))
        finally:
            # Remove files as bot user (Python will clean up tempdir, but we don't necessarily have permissions to clean up files)
            util.rm_as_user("bot_compilation", temp_dir)


def setup_participant(user_index, user_id, temp_dir):
    """Download and set up the bot for a game"""
    bot_dir = "%s_%s" % (user_index, user_id)
    bot_dir = os.path.join(temp_dir, bot_dir)

    os.mkdir(bot_dir)
    bot_archive_path = backend.download_bot(user_id, bot_dir, is_compiled=True)
    archive.unpack(bot_archive_path)

    # Make the start script executable
    os.chmod(os.path.join(bot_dir, 'run.sh'), 0o755)

    # Give the bot user ownership of their directory
    # We should set up each user's default group as a group that the
    # worker is also a part of. Then we always have access to their
    # files, but not vice versa.
    # https://superuser.com/questions/102253/how-to-make-files-created-in-a-directory-owned-by-directory-group

    bot_user = "bot_%s" % user_index
    bot_group = "bot_%s" % user_index
    bot_cgroup = "bot_%s" % user_index

    # We want 770 so that the bot can create files still; leading 2
    # is equivalent to g+s which forces new files to be owned by the group
    util.give_ownership(bot_dir, bot_group, 0o2770)

    bot_command = BOT_COMMAND.format(
        cgroup=bot_cgroup,
        bot_dir=bot_dir,
        bot_group=bot_group,
        bot_user=bot_user,
        runfile='run.sh'
    )

    return bot_command, bot_dir


def run_game(user_ids):
    users = []

    for user_id in user_ids:
        users.append({
            'user_id': user_id,
            'bot_dir': '',
            'bot_logs': '',
        })

    with tempfile.TemporaryDirectory(dir=TEMP_DIR) as temp_dir:
        bomberjam_exec_path = os.path.join(temp_dir, BOMBERJAM_EXEC_NAME)
        game_result_path = os.path.join(temp_dir, 'game.json')

        shutil.copy(BOMBERJAM_EXEC_NAME, bomberjam_exec_path)
        command = [bomberjam_exec_path, '-q', '-o', game_result_path]

        # Make sure bots have access to the temp dir as a whole
        # Otherwise, Python can't import modules from the bot dir
        # Based on strace, Python lstat()s the full dir path to the dir it's in,
        # and fails when it tries to lstat the temp dir, which this fixes
        os.chmod(temp_dir, 0o755)

        # TODO add option for user name

        for user_index, user in enumerate(users):
            bot_command, bot_dir = setup_participant(user_index, user['user_id'], temp_dir)
            command.append(bot_command)
            user['bot_dir'] = bot_dir

        logging.debug("Running game command %s" % command)
        process_stdout = subprocess.Popen(command, stdout=subprocess.PIPE).stdout.read().decode('utf-8')
        logging.debug("Process output:")
        logging.debug(process_stdout)

        with open(game_result_path, 'r') as f:
            game_output = f.read()

        logging.debug("Game output:")
        logging.debug(game_output)

        # tempdir will automatically be cleaned up, but we need to do things
        # manually because the bot might have made files it owns
        for user_index, user in enumerate(users):
            bot_user = "bot_%s" % user_index
            util.rm_as_user(bot_user, temp_dir)

            # The processes won't necessarily be automatically cleaned up, so let's do it ourselves
            util.kill_processes_as(bot_user)

        return game_output


def handle_game_task(user_ids):
    """Downloads compiled bots, runs a game, and posts the results of the game"""
    logging.debug("Running game with users: %s" % ", ".join(map(str, user_ids)))
    run_game(user_ids)

    # Make sure game processes exit
    subprocess.run(["pkill", "--signal", "9", "-f", "cgexec"])


def handle_any_task(task):
    task_id = int(task['id'])
    task_type = int(task['type'])

    try:
        backend.mark_task_started(task_id)

        if task_type == 1:
            user_id = int(task['data'])
            handle_compile_task(user_id)

        elif task_type == 2:
            user_ids = list(map(int, task['data'].split(',')))
            handle_game_task(user_ids)
    finally:
        backend.mark_task_finished(task_id)


def try_handle_next_task():
    try:
        task = backend.get_next_task()
        if task is not None and 'id' in task and 'type' in task and 'data' in task:
            handle_any_task(task)
            return True
    except Exception as ex:
        logging.exception("Error on get task %s" % str(ex))

    return False


def main():
    util.setup_logging()
    logging.info("Starting up worker at %s" % (socket.gethostname()))
    time.sleep(3)

    while True:
        if not try_handle_next_task():
            time.sleep(10)


if __name__ == "__main__":
    main()