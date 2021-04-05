import logging


class Logger:
    def __init__(self):
        self._is_configured = False

    def setup(self, filename):
        if not self._is_configured:
            logging.basicConfig(filename=filename, level=logging.DEBUG, format='%(levelname)s: %(message)s')
            self._is_configured = True

    def debug(self, text):
        if self._is_configured:
            logging.debug(text)

    def info(self, text):
        if self._is_configured:
            logging.info(text)

    def warn(self, text):
        if self._is_configured:
            logging.warning(text)

    def error(self, text):
        if self._is_configured:
            logging.error(text)
