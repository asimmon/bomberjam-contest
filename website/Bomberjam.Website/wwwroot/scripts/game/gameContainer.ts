import { Container } from 'pixi.js';

export default abstract class GameContainer {
  public readonly stateProvider: IStateProvider;
  public readonly container: Container;

  protected constructor(stateProvider: IStateProvider) {
    this.stateProvider = stateProvider;
    this.container = new Container();
  }

  protected get state(): IGameState {
    return this.stateProvider.state;
  }

  public initialize(): void {}

  public onStateChanged(prevState: IGameState): void {}

  public onPixiFrameUpdated(delta: number, totalTime: number): void {}

  public onPlayerAdded(playerId: string, player: IPlayer): void {}

  public onPlayerRemoved(playerId: string, player: IPlayer): void {}

  public onBombAdded(bombId: string, bomb: IBomb): void {}

  public onBombRemoved(bombId: string, bomb: IBomb): void {}

  public onBonusAdded(bonusId: string, bonus: IBonus): void {}

  public onBonusRemoved(bonusId: string, bonus: IBonus): void {}
}
