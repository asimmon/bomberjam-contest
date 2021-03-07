import logging
from datetime import datetime
from pathlib import Path

LOGGING_CONFIGURED = False


def configure_file_logging(file_id):
    """
    Configures the logger to log to a file.

    :param file_id: An id to append to the file name. Useful when you run the same code but you want identifiable log files
    :return: None
    """
    global LOGGING_CONFIGURED

    Path("./logs").mkdir(exist_ok=True)
    logging.basicConfig(filename=__get_logging_file_name__(file_id), level=logging.DEBUG)
    LOGGING_CONFIGURED = True


def log(content):
    """
    Logs the content to file. You must call configure_file_logging before using log.

    :param content: Anything that can be represented as a string
    :return: None
    """
    global LOGGING_CONFIGURED
    if LOGGING_CONFIGURED:
        logging.debug(content)


def __get_logging_file_name__(file_id):
    """
    Composes a logging file name. It contains a timestamp followed by a file id.
    Example: 20210306113741-MyBot-2.log

    :param file_id: An id to append to the file name. Useful when you run the same code but you want identifiable log files
    :return: str
    """
    return f"logs/{datetime.now().strftime('%Y%m%d%H%M%S')}-{file_id}.log"
