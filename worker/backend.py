import json
import logging
import requests
import os
from requests.packages.urllib3.exceptions import InsecureRequestWarning

import util


API_BASE_URL = util.get_env_or_default('API_BASE_URL', 'https://localhost:5001/api/')
API_AUTH = util.get_env_or_default('API_AUTH', 'yolo')

requests.packages.urllib3.disable_warnings(InsecureRequestWarning)


def download_bot(user_id, storage_dir, is_compiled=False):
    """Downloads and stores a bot's zip file locally"""
    logging.debug(f"Downloading {('compiled' if is_compiled else 'source code')} bot for user {user_id}...")

    request_url = API_BASE_URL + (f"user/%s/bot/download?compiled=%s" % (user_id, "true" if is_compiled else "false"))
    request_headers = {'Authorization': 'Secret ' + API_AUTH}

    r = requests.get(request_url, headers=request_headers, verify=False)
    r.raise_for_status()

    local_file_name = "bot-%s-%s.zip" % (user_id, "1" if is_compiled else "0")
    local_file_path = os.path.join(storage_dir, local_file_name)

    if os.path.exists(local_file_path):
        os.remove(local_file_path)

    with open(local_file_path, "wb") as file:
        file.write(r.content)

    logging.debug(f"Downloaded %s bytes" % len(r.content))

    return local_file_path


def upload_bot(user_id, zip_file_path):
    """Posts a bot file to the manager"""
    logging.debug(f"Uploading compiled bot for user {user_id}...")

    with open(zip_file_path, "rb") as f:
        file_bytes = f.read()

    request_url = API_BASE_URL + ("user/%s/bot/upload" % user_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH}

    r = requests.post(request_url, data=file_bytes, headers=request_headers, verify=False)
    r.raise_for_status()

    logging.debug(f"Uploaded %s bytes" % len(file_bytes))


def send_compilation_result(user_id, did_compile, language, errors=None):
    """Posts the result of a compilation task"""
    logging.debug(f"Sending compilation result for user {user_id}...")

    request_url = API_BASE_URL + ("user/%s/bot/compilation-result" % user_id)
    request_headers = {
        'Authorization': 'Secret ' + API_AUTH,
        'Content-Type': 'application/json'
    }
    r = requests.post(request_url, headers=request_headers, verify=False, json={
        'userId': user_id,
        'didCompile': did_compile,
        'language': language,
        'error': str(errors)
    })
    r.raise_for_status()
    logging.debug("Sent compilation result: %s" % r.text)


def get_next_task():
    """Gets either a game run or a compile task from the API"""
    logging.debug(f"Retrieving the next task to execute...")

    request_url = API_BASE_URL + 'task/next'
    request_headers = {'Authorization': 'Secret ' + API_AUTH}

    r = requests.get(request_url, headers=request_headers, verify=False)

    if r.status_code == 404:
        return None

    r.raise_for_status()

    task = json.loads(r.text)
    logging.info("Next task ID: %s, type: %s" % (task['id'], task['type']))
    return task


def mark_task_started(task_id):
    logging.debug(f"Marking task {task_id} as started...")

    request_url = API_BASE_URL + ("task/%s/started" % task_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH}

    r = requests.get(request_url, headers=request_headers, verify=False)
    r.raise_for_status()

    logging.info(f"Marked task: {task_id} as started")


def mark_task_finished(task_id):
    logging.debug(f"Marking task {task_id} as finished...")

    request_url = API_BASE_URL + ("task/%s/finished" % task_id)
    request_headers = {'Authorization': 'Secret ' + API_AUTH}

    r = requests.get(request_url, headers=request_headers, verify=False)
    r.raise_for_status()

    logging.info(f"Marked task: {task_id} as finished")
