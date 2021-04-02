interface Player {
  x: number;
  y: number;
  id: string;
  bombsLeft: number;
  maxBombs: number;
  bombRange: number;
  isAlive: boolean;
  timedOut: boolean;
  score: number;
  color: number;
  respawning: number;
  name: string;
  hasWon: boolean;
  startingCorner: 'tl' | 'tr' | 'bl' | 'br';
}

interface Bomb {
  x: number;
  y: number;
  playerId: string;
  countdown: number;
  range: number;
}

interface Bonus {
  x: number;
  y: number;
  kind: 'bomb' | 'fire';
}

interface State {
  tick: number;
  isFinished: boolean;
  tiles: string;
  players: { [id: string]: Player };
  bombs: { [id: string]: Bomb };
  bonuses: { [id: string]: Bonus };
  width: number;
  height: number;
  suddenDeathCountdown: number;
  isSuddenDeathEnabled: boolean;
}