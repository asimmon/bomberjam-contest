from bomberjam.bomb import Bomb
from bomberjam.bonus import Bonus
from bomberjam.player import Player
from bomberjam.constants import Constants


class State:
    """
    The complete game representation.

    Attributes:
        tiles: Represents the map in a numpy array. You can access any Tile with state.tiles[x, y]
        tick: The current game tick
        is_finished: whether or not the game is finished
        players: A list with all players in the game
        bombs: A list with all bombs in the game
        bonuses: A list with all bonuses in the game
        width: The width of the map
        height: The height of the map
        sudden_death_countdown: When the sudden death countdown reaches 0, the sudden death starts
        is_sudden_death_enabled: True when sudden death is active. During sudden death, respawning is disabled
    """

    def __init__(self, json_state):
        self.tick = int(json_state["tick"])
        self.is_finished = bool(json_state["isFinished"])
        self.players = dict((player_id, Player(player_json)) for (player_id, player_json) in json_state["players"].items())
        self.bombs = dict((bomb_id, Bomb(bomb_json)) for (bomb_id, bomb_json) in json_state["bombs"].items())
        self.bonuses = dict((bonus_id, Bonus(bonus_json)) for (bonus_id, bonus_json) in json_state["bonuses"].items())
        self.width = int(json_state["width"])
        self.height = int(json_state["height"])
        self.sudden_death_countdown = int(json_state["suddenDeathCountdown"])
        self.is_sudden_death_enabled = bool(json_state["isSuddenDeathEnabled"])
        self.tiles = str(json_state["tiles"])

    def get_tile_at(self, x, y) -> str:
        if self.is_out_of_bounds(x, y):
            return Constants.OUT_OF_BOUNDS
        return self.tiles[self.coord_to_tile_index(x, y)]

    def is_out_of_bounds(self, x, y):
        return x < 0 or y < 0 or x >= self.width or y >= self.height

    def coord_to_tile_index(self, x, y):
        return y * self.width + x

    def find_active_bomb_at(self, x, y) -> Bomb or None:
        return next((b for b in self.bombs.values() if b.countdown > 0 and b.x == x and b.y == y), None)

    def find_dropped_bonus_at(self, x, y) -> Bonus or None:
        return next((b for b in self.bonuses.values() if b.x == x and b.y == y), None)

    def find_alive_player_at(self, x, y) -> Player or None:
        return next((p for p in self.players.values() if p.is_alive and p.x == x and p.y == y), None)
