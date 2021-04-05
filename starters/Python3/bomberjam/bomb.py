class Bomb:
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
        self.countdown = int(bomb_json["countdown"])
        self.player_id = str(bomb_json["playerId"])
        self.range = int(bomb_json["range"])
        self.x = int(bomb_json["x"])
        self.y = int(bomb_json["y"])
