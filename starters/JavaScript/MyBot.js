const process = require('process');
const bomberjam = require('./bomberjam');

const allActions = [
  bomberjam.Constants.Left,
  bomberjam.Constants.Up,
  bomberjam.Constants.Right,
  bomberjam.Constants.Down,
  bomberjam.Constants.Stay,
  bomberjam.Constants.Bomb
];

const game = new bomberjam.Game();

// Standard output (console.log) can ONLY BE USED to communicate with the bomberjam process
// Use text files if you need to log something for debugging
const logger = new bomberjam.Logger();

// Edit run_game.(bat|sh) to include file logging for any of the four bot processes: node MyBot.js --logging
if (process.argv.slice(2).some(x => x.toLowerCase() === '--logging'))
  logger.setup(`log-${new Date().getTime()}.log`);

const main = async () => {
  // 1) You must send an alphanumerical name up to 32 characters
  // Spaces or special characters are not allowed
  await game.ready('MyName' + Math.round(Math.random() * (9999 - 1000) + 1000));
  logger.info('My player ID is ' + game.myPlayerId);

  do {
    // 2) Each tick, you'll receive the current game state serialized as JSON
    // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    await game.receiveCurrentState();

    try {
      // 3) Analyze the current state and decide what to do
      const state = game.state;

      for (let x = 0; x < state.width; x++) {
        for (let y = 0; y < state.height; y++) {
          const tile = bomberjam.StateUtils.getTileAt(state, x, y);
          if (tile === bomberjam.Constants.Block) {
            // TODO found a block to destroy
          }

          const otherPlayer = bomberjam.StateUtils.findAlivePlayerAt(state, x, y);
          if (otherPlayer && otherPlayer.id !== game.myPlayerId) {
            // TODO found an alive opponent
          }

          const bomb = bomberjam.StateUtils.findActiveBombAt(state, x, y);
          if (bomb) {
            // TODO found an active bomb
          }

          const bonus = bomberjam.StateUtils.findDroppedBonusAt(state, x, y);
          if (bonus) {
            // TODO found a bonus
          }
        }
      }

      if (game.myPlayer.bombsLeft > 0) {
        // TODO you can drop a bomb
      }

      // 4) Send your action
      const action = allActions[Math.floor(Math.random() * allActions.length)];
      game.sendAction(action);
      logger.info('Tick ' + state.tick + ', sent action: ' + action);
    } catch (err) {
      // Handle your exceptions per tick
      logger.error('Tick ' + game.state.tick + ', exception: ' + err.toString());
    }
  } while (game.myPlayer.isAlive && !game.state.isFinished);
};

main().catch(console.err).finally(() => {
  logger.close();
  game.close();
});
