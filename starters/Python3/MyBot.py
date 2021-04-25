import random
import sys
import time

from bomberjam.constants import Constants
from bomberjam.game import Game
from bomberjam.logger import Logger


game = Game()

# Standard output (print) can ONLY BE USED to communicate with the bomberjam process
# Use text files if you need to log something for debugging
logger = Logger()

# Edit run_game.(bat|sh) to include file logging for any of the four bot processes: python3 MyBot.py --logging
if True in (arg.lower() == '--logging' for arg in sys.argv):
    logger.setup('log-' + str(round(time.time() * 1000)) + '.log')

# 1) You must send an alphanumerical name up to 32 characters
# Spaces or special characters are not allowed
game.ready("MyName" + str(random.randint(1000, 9999)))
logger.info('My player ID is ' + game.my_player_id)

while True:
    # 2) Each tick, you'll receive the current game state serialized as JSON
    # From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    game.receive_current_state()

    try:
        # 3) Analyze the current state and decide what to do
        for x in range(game.state.width):
            for y in range(game.state.height):
                tile = game.state.get_tile_at(x, y)
                if tile == Constants.BLOCK:
                    pass  # TODO found a block to destroy

                other_player = game.state.find_alive_player_at(x, y)
                if other_player is not None and other_player.id != game.my_player_id:
                    pass  # TODO found an alive opponent

                bomb = game.state.find_active_bomb_at(x, y)
                if bomb is not None:
                    pass  # TODO found an active bomb

                bonus = game.state.find_dropped_bonus_at(x, y)
                if bonus is not None:
                    pass  # TODO found a bonus

        if game.my_player.bombs_left > 0:
            pass  # TODO you can drop a bomb

        # 4) Send your action
        action = random.choice([Constants.UP, Constants.DOWN, Constants.LEFT, Constants.RIGHT, Constants.STAY, Constants.BOMB])
        game.send_action(action)
        logger.info('Tick ' + str(game.state.tick) + ', sent action: ' + action)
    except Exception as ex:
        # Handle your exceptions per tick
        logger.error('Tick ' + str(game.state.tick) + ', exception: ' + str(ex))

    if not game.my_player.is_alive or game.state.is_finished:
        break
