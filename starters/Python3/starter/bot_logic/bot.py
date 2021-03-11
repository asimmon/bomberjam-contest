import random
from core.logging import log
from models.action import Action


class Bot:
    """
    Your Bomberjam bot.
    NAME should be your bot name. It cannot contain spaces or special characters.
    compute_next_action(state) should return an Action given a state.
    You can also add anything you need!
    """
    NAME = "MyBot"

    def __init__(self, bot_id):
        self.bot_id = bot_id

    def compute_next_action(self, state):
        """
        Computes the next action your bot should do based on the current game state.

        :param state: The current game state
        :return: Action
        """
        log(state)
        my_bot = state.my_bot
        log(state.tiles[my_bot.x, my_bot.y])

        return random.choice(Action.tolist())
