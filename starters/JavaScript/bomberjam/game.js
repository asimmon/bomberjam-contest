const rl = require('readline').createInterface({
  input: process.stdin,
  output: process.stdout,
});

const readlineAsync = (function () {
  const readlineGen = (async function* () {
    for await (const line of rl) {
      yield line;
    }
  })();
  return async () => ((await readlineGen.next()).value);
})();

class Game {
  constructor() {
    this._isReady = false;
    this._myPlayerId = null;

    /** @type {State} */
    this._state = null;
  }

  /** @returns {string} */
  get myPlayerId() {
    this._ensureIsReady();
    return this._myPlayerId;
  }

  /** @returns {State} */
  get state() {
    this._ensureIsReady();
    this._ensureInitialState();
    return this._state;
  }

  /** @returns {Player} */
  get myPlayer() {
    this._ensureIsReady();
    this._ensureInitialState();
    return this._state.players[this._myPlayerId];
  }

  /**
   * @param {string} playerName
   * @returns {Promise<void>}
   */
  async ready(playerName) {
    if (this._isReady)
      return;

    if (typeof playerName !== 'string' || playerName.trim().length === 0)
      throw new Error('Your name cannot be null or empty');

    console.log('0:' + playerName);

    this._myPlayerId = await readlineAsync();

    if (!/^\d+$/.test(this._myPlayerId))
      throw new Error('Could not retrieve your ID from standard input');

    this._isReady = true;
  }

  /** @returns {Promise<void>} */
  async receiveCurrentState() {
    this._ensureIsReady();
    const jsonState = await readlineAsync();
    this._state = JSON.parse(jsonState);
  }

  /** @param {string} action */
  sendAction(action) {
    this._ensureIsReady();
    this._ensureInitialState();

    console.log(this._state.tick + ':' + action);
  }

  _ensureIsReady() {
    if (!this._isReady)
      throw new Error('You need to call Game.ready(...) with your name first');
  }

  _ensureInitialState() {
    if (!this._state)
      throw new Error('You need to call Game.receiveCurrentState() to retrieve the initial state of the game');
  }

  close() {
    rl.close();
  }
}

module.exports = Game;