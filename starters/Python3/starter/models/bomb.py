from core.json_serializable import JSONSerializable


class Bomb(JSONSerializable):
    """
    The game representation of a bomb.

    Attributes:
        countdown: A bomb explodes when its countdown reaches 0
        player_id: The id of the played who owns this bomb
        range: Represents how far the bomb explosion can reach. A range of 0 will only burn the tile where the bomb is
        x: The x coordinate of the bomb
        y: The y coordinate of the bomb
    """

    def __init__(self, bomb_json):
        """
        :param bomb_json: A bomb in json formatted dict
        """
        self.countdown = bomb_json["countdown"]
        self.player_id = bomb_json["playerId"]
        self.range = bomb_json["range"]
        self.x = bomb_json["x"]
        self.y = bomb_json["y"]
