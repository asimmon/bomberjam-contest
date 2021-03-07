from core.enumerable_enum import Enumerable


class Action(Enumerable):
    """
    Represents all actions that a bot can do in the game.

    Actions:
        UP: Go up
        DOWN: Go down
        LEFT: Go left
        RIGHT: Go right
        STAY: Do nothing
        BOMB: Drop a bomb at the player's position
    """

    UP = "up"
    DOWN = "down"
    LEFT = "left"
    RIGHT = "right"
    STAY = "stay"
    BOMB = "bomb"
