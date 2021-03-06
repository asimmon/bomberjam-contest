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
    cmd_args = ["sudo", "-H", "-u", username, "-s", "/bin/pkill", "-9", "-u", username]
    if process_name is not None:
        cmd_args.append(process_name)
    subprocess.call(cmd_args, stderr=subprocess.PIPE, stdout=subprocess.PIPE)


def give_ownership(top_dir, dir_perms, group):
    """Give ownership of everything in a directory to a given group."""
    for dirpath, _, filenames in os.walk(top_dir):
        shutil.chown(dirpath, group=group)
        os.chmod(dirpath, dir_perms)
        for filename in filenames:
            shutil.chown(os.path.join(dirpath, filename), group=group)
            os.chmod(os.path.join(dirpath, filename), dir_perms)


def rm_as_user(user, directory):
    """Remove a directory tree as the specified user."""
    args = ["sudo", "-H", "-u", user, "-s", "/bin/rm", "-rf", directory]
    subprocess.call(args, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)


def rm_everything_owned_by(user, directory):
    """Remove everything owned by the specified user excluding the directory itself."""
    args = ["sudo", "-H", "-u", user, "-s", "/bin/find", directory, "-mindepth", "1", "-user", user, "-exec", "/bin/rm", "-rf", "{}", ";"]
    subprocess.call(args, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)


def restore_user_default_profile(user):
    """Restore ~ default files: .profile, .bashrc and .bash_logout"""
    home_dir = "/home/%s/" % user
    args1 = ["sudo", "-H", "-u", user, "-s", "/bin/cp", "/etc/skel/.profile", home_dir]
    args2 = ["sudo", "-H", "-u", user, "-s", "/bin/cp", "/etc/skel/.bashrc", home_dir]
    args3 = ["sudo", "-H", "-u", user, "-s", "/bin/cp", "/etc/skel/.bash_logout", home_dir]
    subprocess.call(args1, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)
    subprocess.call(args2, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)
    subprocess.call(args3, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)


def chmod_recursive(user, directory, permissions):
    try:
        args = ["sudo", "-H", "-u", user, "-s", "/bin/find", directory, "-user", user, "-exec", "/bin/chmod", "-R", permissions, "{}", ";"]
        subprocess.call(args, stderr=subprocess.DEVNULL, stdout=subprocess.DEVNULL)
    except:
        pass


def rm_everything_owned_in_tmp_and_home_by(user):
    """Remove everything owned by the specified user in /tmp and its /home/<user> directory, excluding these directories."""
    # Ensure that we can delete user-created files if they were set read-only
    chmod_recursive(user, "/tmp/", "755")
    chmod_recursive(user, "/home/%s/" % user, "755")

    rm_everything_owned_by(user, "/tmp/")
    rm_everything_owned_by(user, "/home/%s/" % user)

    restore_user_default_profile(user)


def setup_logging():
    logging.getLogger('requests').setLevel(logging.WARNING)
    logging.getLogger('urllib3').setLevel(logging.WARNING)

    stdout_handler = logging.StreamHandler(sys.stdout)
    stdout_handler.setLevel(logging.DEBUG)
    stdout_handler.setFormatter(logging.Formatter('%(asctime)s [%(levelname)s]: %(message)s'))
    logging.getLogger().setLevel(logging.DEBUG)
    logging.getLogger().addHandler(stdout_handler)


class Player:
    def __init__(self, player_index, player_id, player_name):
        self.player_index = player_index
        self.player_id = player_id
        self.player_name = player_name
        self.bot_id = ''
        self.bot_dir = ''
        self.bot_logs = ''


class Game:
    # serialized_player_data format: origin#id:name,id:name,id:name,id:name
    # Example: 2#1:foo,5:bar,6:qux,4:baz
    def __init__(self, serialized_game_data):
        self.players = []
        self.game_result = ''
        self.game_stdout = ''
        self.game_stderr = ''
        self.players_bot_ids = {}

        self.origin = int(serialized_game_data[0:1])
        serialized_player_data = serialized_game_data[2:]

        player_data = {k: str(v) for k, v in [i.split(':') for i in serialized_player_data.split(',')]}
        for player_index, player_id in enumerate(player_data):
            self.add_player(player_index, player_id, player_data[player_id])

    def add_player(self, player_index, player_id, player_name):
        player = Player(player_index, player_id, player_name)
        self.players.append(player)


class Expando(object):
    pass
