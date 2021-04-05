class Bonus:
    """
    The game representation of a bonus.

    Attributes:
        kind: The kind of bonus
        x: The x coordinate of the bonus
        y: The y coordinate of the bonus
    """

    def __init__(self, bonus_json):
        self.kind = str(bonus_json["kind"])
        self.x = int(bonus_json["x"])
        self.y = int(bonus_json["y"])
