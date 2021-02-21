// Standard output (console.log) can ONLY BE USED to communicate with the bomberjam process
// Use text files if you need to log something for debugging

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

const main = async () => {
  // 1) You must send an alphanumerical name up to 32 characters, prefixed by "0:"
  // No spaces or special characters are allowed
  console.log('0:MyName' + (Math.random() * (9999 - 1000) + 1000));

  // 2) Receive your player ID from the standard input
  const myPlayerId = await readlineAsync();

  while (true) {
    // 3) Each tick, you'll receive the current game state serialized as JSON
    // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    const jsonState = await readlineAsync();
    const state = JSON.parse(jsonState);

    try {
      // 4) Send your action prefixed by the current tick number and a colon
      const allActions = ['left', 'up', 'right', 'down', 'stay', 'bomb'];
      const randomAction = allActions[Math.floor(Math.random() * allActions.length)];
      console.log(state.tick + ':' + randomAction)
    } catch (err) {
      // Handle your exceptions per tick
    }
  }
};

main().catch(console.err).finally(rl.close);
