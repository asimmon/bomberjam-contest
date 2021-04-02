const Constants = require('./constants');

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 */
const isOutOfBounds = (state, x, y) => {
  return x < 0 || y < 0 || x >= state.width || y >= state.height;
};

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 */
const coordToTileIndex = (state, x, y) => {
  return y * state.width + x;
};

class StateUtils {
}

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 * @returns {string}
 */
StateUtils.getTileAt = (state, x, y) => {
  return isOutOfBounds(state, x, y) ? Constants.OutOfBounds : state.tiles[coordToTileIndex(state, x, y)];
};

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 * @returns {Bomb|undefined}
 */
StateUtils.findActiveBombAt = (state, x, y) => {
  for (let bombId in state.bombs) {
    const bomb = state.bombs[bombId];
    if (bomb.countdown > 0 && bomb.x === x && bomb.y === y)
      return bomb;
  }
};

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 * @returns {Player|undefined}
 */
StateUtils.findAlivePlayerAt = (state, x, y) => {
  for (let playerId in state.players) {
    const player = state.players[playerId];
    if (player.isAlive && player.x === x && player.y === y)
      return player;
  }
};

/**
 * @param {State} state
 * @param {number} x
 * @param {number} y
 * @returns {Bonus|undefined}
 */
StateUtils.findDroppedBonusAt = (state, x, y) => {
  for (let bonusId in state.bonuses) {
    const bonus = state.bonuses[bonusId];
    if (bonus.x === x && bonus.y === y)
      return bonus;
  }
};

module.exports = StateUtils;