import logging
import os
import shutil
import subprocess
import sys


def get_env_or_default(key, default):
    val = os.getenv(key)
    return default if val is None else val


def kill_processes_as(username, process_name=None):
    """Kill all processes of a given name belonging to a given user."""
    cmd_args = ["sudo", "-H", "-u", username, "-s", "killall", "-9", "-u", username]
    if process_name is not None:
        cmd_args.append(process_name)
    subprocess.call(cmd_args, stderr=subprocess.PIPE, stdout=subprocess.PIPE)


def give_ownership(top_dir, group, dir_perms):
    """Give ownership of everything in a directory to a given group."""
    for dirpath, _, filenames in os.walk(top_dir):
        shutil.chown(dirpath, group=group)
        os.chmod(dirpath, dir_perms)
        for filename in filenames:
            shutil.chown(os.path.join(dirpath, filename), group=group)
            os.chmod(os.path.join(dirpath, filename), dir_perms)


def rm_as_user(user, directory):
    """Remove a directory tree as the specified user."""
    subprocess.call(
        ["sudo", "-H", "-u", user, "-s", "rm", "-rf", directory],
        stderr=subprocess.DEVNULL,
        stdout=subprocess.DEVNULL)


def setup_logging():
    logging.getLogger('requests').setLevel(logging.WARNING)
    logging.getLogger('urllib3').setLevel(logging.WARNING)

    stdout_handler = logging.StreamHandler(sys.stdout)
    stdout_handler.setLevel(logging.DEBUG)
    stdout_handler.setFormatter(logging.Formatter('%(asctime)s [%(levelname)s]: %(message)s'))
    logging.getLogger().setLevel(logging.DEBUG)
    logging.getLogger().addHandler(stdout_handler)
