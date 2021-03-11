import json
import logging
import os
import requests
import shutil
from requests.packages.urllib3.exceptions import InsecureRequestWarning

import util


API_BASE_URL = util.get_env_or_default('API_BASE_URL', 'https://localhost:5001/api/')
API_AUTH_TOKEN = util.get_env_or_default('API_AUTH_TOKEN', 'yolo')
API_VERIFY_SSL = util.get_env_or_default('API_VERIFY_SSL', '1') == '1'

requests.packages.urllib3.disable_warnings(InsecureRequestWarning)


def download_bot(bot_id, storage_dir, is_compiled=False):
    """Downloads and stores a bot's zip file locally"""
    logging.debug(f"Downloading {('compiled' if is_compiled else 'source code')} bot {bot_id}...")

    local_file_name = "bot-%s-%s.zip" % (bot_id, "1" if is_compiled else "0")
    local_file_path = os.path.join(storage_dir, local_file_name)

    request_url = API_BASE_URL + (f"bot/%s/download?compiled=%s" % (bot_id, "true" if is_compiled else "false"))
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with requests.get(request_url, headers=request_headers, verify=API_VERIFY_SSL, stream=True) as r:
        r.raw.decode_content = True
        r.raise_for_status()

        if os.path.exists(local_file_path):
            os.remove(local_file_path)

        with open(local_file_path, "wb") as f:
            shutil.copyfileobj(r.raw, f)

    logging.debug(f"Downloaded {os.path.getsize(local_file_path)} bytes on disk")

    return local_file_path


def upload_bot(bot_id, zip_file_path):
    """Posts a bot file to the website"""
    logging.debug(f"Uploading compiled bot {bot_id} of size {os.path.getsize(zip_file_path)} bytes on disk...")

    request_url = API_BASE_URL + ("bot/%s/upload" % bot_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with open(zip_file_path, "rb") as f:
        with requests.post(request_url, data=f, headers=request_headers, verify=API_VERIFY_SSL) as r:
            r.raise_for_status()

    logging.debug("Uploaded")


def send_compilation_result(bot_id, did_compile, language, compilation_errors):
    """Posts the result of a compilation task"""
    logging.debug(f"Sending compilation result for bot {bot_id}...")

    request_url = API_BASE_URL + ("bot/%s/compilation-result" % bot_id)
    request_headers = {
        'Authorization': 'Secret ' + API_AUTH_TOKEN,
        'Content-Type': 'application/json'
    }
    request_data = {
        'botId': bot_id,
        'didCompile': True if did_compile else False,
        'language': language,
        'errors': compilation_errors
    }
    with requests.post(request_url, headers=request_headers, verify=API_VERIFY_SSL, json=request_data) as r:
        r.raise_for_status()
        logging.debug("Sent compilation result: %s" % r.text)


def get_player_compiled_bot_id(player_id):
    logging.debug(f"Retrieving the latest compiled bot ID for player {player_id}...")

    request_url = API_BASE_URL + ("user/%s/bot" % player_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with requests.get(request_url, headers=request_headers, verify=API_VERIFY_SSL) as r:
        r.raise_for_status()
        bot_id = r.text

    logging.info("Latest compiled bot ID for player %s: %s" % (player_id, bot_id))
    return bot_id


def send_game_result(game):
    logging.debug("Sending game result")

    request_url = API_BASE_URL + "game"
    request_headers = {
        'Authorization': 'Secret ' + API_AUTH_TOKEN,
        'Content-Type': 'application/json'
    }
    request_data = {
        'serializedHistory': game.game_result,
        'standardOutput': game.game_stdout,
        'standardError': game.game_stderr,
        'playerBotIds': game.players_bot_ids,
        'origin': game.origin
    }
    with requests.post(request_url, headers=request_headers, verify=API_VERIFY_SSL, json=request_data) as r:
        r.raise_for_status()
        logging.debug("Sent game result: %s" % r.text)


def get_next_task():
    """Gets either a game run or a compile task from the API"""
    logging.debug(f"Retrieving the next task to execute...")

    request_url = API_BASE_URL + 'task/next'
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with requests.get(request_url, headers=request_headers, verify=API_VERIFY_SSL) as r:
        if r.status_code == 404:
            return None

        r.raise_for_status()
        task = json.loads(r.text)
        logging.info("Next task ID: %s, type: %s" % (task['id'], task['type']))
        return task


def mark_task_started(task_id):
    logging.debug(f"Marking task {task_id} as started...")

    request_url = API_BASE_URL + ("task/%s/started" % task_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with requests.get(request_url, headers=request_headers, verify=API_VERIFY_SSL) as r:
        r.raise_for_status()

    logging.info(f"Marked task: {task_id} as started")


def mark_task_finished(task_id):
    logging.debug(f"Marking task {task_id} as finished...")

    request_url = API_BASE_URL + ("task/%s/finished" % task_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH_TOKEN}

    with requests.get(request_url, headers=request_headers, verify=API_VERIFY_SSL) as r:
        r.raise_for_status()

    logging.info(f"Marked task: {task_id} as finished")
