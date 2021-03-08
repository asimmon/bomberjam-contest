# You CANNOT rename this file.
#
# Contains the game loop.
# You shouldn't have to modify this file unless you have specific needs.
# Your bot logic should be implemented in bot_logic/bot.py.
# You can, however, do anything you'd like.
#
# This script takes one optional argument: --logging
# Example usage: python MyBot.py --logging=True
# ==============================================================================

import argparse

from bot_logic.bot import Bot
from core.commands import ActionCommand
from core.commands import RegisterBotCommand
from core.logging import configure_file_logging
from core.logging import log
from models.state import State


def is_logging_enabled():
    """
    Gets the --logging argument

    :return: bool
    """
    parser = argparse.ArgumentParser()
    parser.add_argument("--logging", help="Activate logging", default=False)

    return parser.parse_args().logging


def play():
    """
    Abstracts the game loop and the configurations required.
    You will need to implement bot_logic/bot.py for this to work properly.

    :return: None
    """
    bot_name = Bot.NAME

    print(RegisterBotCommand(bot_name))
    bot_id = input()
    bot = Bot(bot_id)

    if is_logging_enabled():
        configure_file_logging(f"MyBot-{bot_id}")

    log(f"Bot name is '{bot_name}' with id '{bot_id}'")

    state = State(input(), bot_id)
    while not state.is_finished:
        try:
            tick = state.tick
            action = bot.compute_next_action(state)

            print(ActionCommand(tick, action))
            state = State(input(), bot_id)
        except Exception as error:
            log(error)
            break


play()
