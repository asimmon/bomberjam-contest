from core.enumerable_enum import Enumerable
from core.json_serializable import JSONSerializable


class Bonus(JSONSerializable):
    """
    The game representation of a bonus.

    Attributes:
        kind: The Kind of bonus
        x: The x coordinate of the bonus
        y: The y coordinate of the bonus
    """

    def __init__(self, bonus_json):
        """
        :param bonus_json: A bonus in json formatted dict
        """
        self.kind = bonus_json["kind"]
        self.x = bonus_json["x"]
        self.y = bonus_json["y"]


class BonusKind(Enumerable):
    """
    The different kinds of bonuses.
    
    Kinds:
        BOMB_COUNT: Allows a player to use more bombs at the same time
        BOMB_RANGE: Makes the bomb explosion reach farther
    """

    BOMB_COUNT = "bomb"
    BOMB_RANGE = "fire"
