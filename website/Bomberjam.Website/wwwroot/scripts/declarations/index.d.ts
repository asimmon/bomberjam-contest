interface JSON {
  parse<T = any>(text: string): T;
}

interface Document {
  getElementById<T extends HTMLElement>(elementId: string): T | null;
}

interface ISetTimeout {
  (callback: () => void, delay: number): void;
}

interface IHasPos {
  x: number;
  y: number;
}

interface IPlayer extends IHasPos {
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
}

interface IBomb extends IHasPos {
  playerId: string;
  countdown: number;
  range: number;
}

interface IBonus extends IHasPos {
  kind: 'bomb' | 'fire';
}

interface IGameState {
  id: string;
  tick: number;
  isFinished: boolean;
  tiles: string;
  players: { [id: string]: IPlayer };
  bombs: { [id: string]: IBomb };
  bonuses: { [id: string]: IBonus };
  width: number;
  height: number;
  suddenDeathCountdown: number;
  isSuddenDeathEnabled: boolean;
  tickDuration: number;
}

interface ITickHistory {
  state: IGameState;
  actions: { [id: string]: string | null };
}

interface IPlayerErrorHistory {
  playerId: string;
  tick: number | null;
  error: string;
}

interface IGameConfiguration {
  suddenDeathCountdown: number;
  respawnTime: number;
}

interface IGameHistory {
  configuration: IGameConfiguration;
  errors: IPlayerErrorHistory[];
  ticks: ITickHistory[];
}

interface IStateProvider {
  state: IGameState;
  configuration: IGameConfiguration;
}

interface IReplayGameController {
  destroy: () => void;
  resumeGame: () => void;
  pauseGame: () => void;
  increaseSpeed: () => void;
  decreaseSpeed: () => void;
  goToStateIdx: (newStateIdx: number) => void;
}
