import json

from bomberjam.player import Player
from bomberjam.state import State


class Game:
    def __init__(self):
        self._is_ready = False
        self._my_player_id = None
        self._state = None

    @property
    def my_player_id(self) -> str:
        self.ensure_is_ready()
        return self._my_player_id

    @property
    def state(self) -> State:
        self.ensure_is_ready()
        self.ensure_initial_state()
        return self._state

    @property
    def my_player(self) -> Player:
        self.ensure_is_ready()
        self.ensure_initial_state()
        return self._state.players[self._my_player_id]

    def ready(self, player_name):
        if self._is_ready:
            return

        if player_name is None or len(player_name.strip()) == 0:
            raise ValueError('Your name cannot be null or empty')

        print('0:' + player_name, flush=True)

        self._my_player_id = input()
        if not self._my_player_id.isdigit():
            raise RuntimeError('Could not retrieve your ID from standard input')

        self._is_ready = True

    def receive_current_state(self):
        self.ensure_is_ready()
        json_state = json.loads(input())
        self._state = State(json_state)

    def send_action(self, action):
        self.ensure_is_ready()
        self.ensure_initial_state()
        print(str(self._state.tick) + ':' + action, flush=True)

    def ensure_is_ready(self):
        if not self._is_ready:
            raise RuntimeError('You need to call Game.ready(...) with your name first')

    def ensure_initial_state(self):
        if self._state is None:
            raise RuntimeError('You need to call Game.receiveCurrentState() to retrieve the initial state of the game')
