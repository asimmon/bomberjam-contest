class Player:
    """
    The game representation of a player.

    Attributes:
        id: The id of the player
        name: The name of the player
        x: The x coordinate of the player
        y: The y coordinate of the player
        starting_corner: The starting corner of a player
        bombs_left: The number of bombs a player can still use
        max_bombs: The maximum number of bombs a player can use at a time
        bomb_range: The range of this player's bombs
        is_alive: This will be False when the player is eliminated during the sudden death period
        timed_out: True if the player failed to give an Action
        respawning: The time before the player respawns
        score: The score of the player
        color: The color of the player
    """

    def __init__(self, player_json):
        self.id = str(player_json["id"])
        self.name = str(player_json["name"])
        self.x = int(player_json["x"])
        self.y = int(player_json["y"])
        self.starting_corner = str(player_json["startingCorner"])
        self.bombs_left = int(player_json["bombsLeft"])
        self.max_bombs = int(player_json["maxBombs"])
        self.bomb_range = int(player_json["bombRange"])
        self.is_alive = bool(player_json["isAlive"])
        self.timed_out = bool(player_json["timedOut"])
        self.respawning = int(player_json["respawning"])
        self.score = int(player_json["score"])
        self.color = int(player_json["color"])
