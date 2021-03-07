from core.enumerable_enum import Enumerable


class Tile(Enumerable):
    """
    Represents all tile types in the game. Note that Tiles do not represent if a player, a bomb, or a bonus is present.

    Tiles:
        EMPTY: An empty tile
        EXPLOSION: A bomb explosion. Will kill players
        BLOCK: A breakable tile
        WALL: An unbreakable tile
    """

    EMPTY = "."
    EXPLOSION = "*"
    BLOCK = "+"
    WALL = "#"
