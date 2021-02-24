import { Application } from 'pixi.js'
import GameHud from './gameHud';
import GameMap from './gameMap';
import TextureRegistry from './textureRegistry';

export default class BomberjamRenderer {
  private readonly stateProvider: IStateProvider;
  private readonly pixiApp: Application;
  private readonly textures: TextureRegistry;
  private prevState: IGameState;
  private totalTime: number;

  private readonly map: GameMap;
  private readonly hud: GameHud;
  private readonly isReplay: boolean;

  constructor(
    stateProvider: IStateProvider,
    pixiApp: Application,
    textures: TextureRegistry,
    isReplay: boolean = false
  ) {
    this.stateProvider = stateProvider;
    this.pixiApp = pixiApp;
    this.textures = textures;
    this.isReplay = isReplay;

    this.prevState = this.stateProvider.state;
    this.totalTime = 0;
    this.map = new GameMap(stateProvider, textures);
    this.hud = new GameHud(stateProvider, textures);

    this.initialize();
  }

  public initialize() {
    this.map.initialize();
    this.hud.initialize();

    this.pixiApp.stage.addChild(this.map.container);
    this.pixiApp.stage.addChild(this.hud.container);
    this.hud.container.position.x = this.map.container.width;

    this.registerStateChangeHandlers();

    this.pixiApp.renderer.resize(this.pixiApp.stage.width, this.pixiApp.stage.height);
  }

  public registerStateChangeHandlers() {
    this.addPlayerListeners();
    this.addBombsListeners();
    this.addBonusesListeners();
  }

  private addPlayerListeners() {
    // onAdd and onRemove are only available on Colyseus State derivatives
    if (!this.isReplay) {
      (this.stateProvider.state.players as any).onAdd = (player: IPlayer, playerId: string) => {
        this.map.onPlayerAdded(playerId, player);
        this.hud.onPlayerAdded(playerId, player);
      };

      (this.stateProvider.state.players as any).onRemove = (player: IPlayer, playerId: string) => {
        this.map.onPlayerRemoved(playerId, player);
        this.hud.onPlayerRemoved(playerId, player);
      };
    }

    for (const playerId in this.stateProvider.state.players) {
      this.map.onPlayerAdded(playerId, this.stateProvider.state.players[playerId]);
      this.hud.onPlayerAdded(playerId, this.stateProvider.state.players[playerId]);
    }
  }

  private addBombsListeners() {
    if (!this.isReplay) {
      (this.stateProvider.state.bombs as any).onAdd = (bomb: IBomb, bombId: string) => {
        this.map.onBombAdded(bombId, bomb);
        this.hud.onBombAdded(bombId, bomb);
      };

      (this.stateProvider.state.bombs as any).onRemove = (bomb: IBomb, bombId: string) => {
        this.map.onBombRemoved(bombId, bomb);
        this.hud.onBombRemoved(bombId, bomb);
      };
    }

    for (const bombId in this.stateProvider.state.bombs) {
      this.map.onBombAdded(bombId, this.stateProvider.state.bombs[bombId]);
      this.hud.onBombAdded(bombId, this.stateProvider.state.bombs[bombId]);
    }
  }

  private addBonusesListeners() {
    if (!this.isReplay) {
      (this.stateProvider.state.bonuses as any).onAdd = (bonus: IBonus, bonusId: string) => {
        this.map.onBonusAdded(bonusId, bonus);
        this.hud.onBonusAdded(bonusId, bonus);
      };

      (this.stateProvider.state.bonuses as any).onRemove = (bonus: IBonus, bonusId: string) => {
        this.map.onBonusRemoved(bonusId, bonus);
        this.hud.onBonusRemoved(bonusId, bonus);
      };
    }

    for (const bonusId in this.stateProvider.state.bonuses) {
      this.map.onBonusAdded(bonusId, this.stateProvider.state.bonuses[bonusId]);
      this.hud.onBonusAdded(bonusId, this.stateProvider.state.bonuses[bonusId]);
    }
  }

  public onStateChanged() {
    if (this.prevState) {
      if (this.isReplay) {
        this.addNewAndRemoveOldBombsForReplay();
        this.addNewAndRemoveOldBonusesForReplay();
      }

      this.map.onStateChanged(this.prevState);
      this.hud.onStateChanged(this.prevState);
    }

    this.prevState = JSON.parse(JSON.stringify(this.stateProvider.state));
  }

  private readonly removedBombIds = new Set<string>();

  private addNewAndRemoveOldBombsForReplay() {
    this.removedBombIds.clear();

    for (const bombId in this.prevState.bombs) {
      this.removedBombIds.add(bombId);
    }

    for (const bombId in this.stateProvider.state.bombs) {
      if (this.removedBombIds.has(bombId)) {
        this.removedBombIds.delete(bombId);
      } else {
        this.map.onBombAdded(bombId, this.stateProvider.state.bombs[bombId]);
        this.hud.onBombAdded(bombId, this.stateProvider.state.bombs[bombId]);
      }
    }

    for (const removedBombId of this.removedBombIds) {
      this.map.onBombRemoved(removedBombId, this.prevState.bombs[removedBombId]);
      this.hud.onBombRemoved(removedBombId, this.prevState.bombs[removedBombId]);
    }
  }

  private readonly removedBonusIds = new Set<string>();

  private addNewAndRemoveOldBonusesForReplay() {
    this.removedBonusIds.clear();

    for (const bonusId in this.prevState.bonuses) {
      this.removedBonusIds.add(bonusId);
    }

    for (const bonusId in this.stateProvider.state.bonuses) {
      if (this.removedBonusIds.has(bonusId)) {
        this.removedBonusIds.delete(bonusId);
      } else {
        this.map.onBonusAdded(bonusId, this.stateProvider.state.bonuses[bonusId]);
        this.hud.onBonusAdded(bonusId, this.stateProvider.state.bonuses[bonusId]);
      }
    }

    for (const removedBonusId of this.removedBonusIds) {
      this.map.onBonusRemoved(removedBonusId, this.prevState.bonuses[removedBonusId]);
      this.hud.onBonusRemoved(removedBonusId, this.prevState.bonuses[removedBonusId]);
    }
  }

  public onPixiFrameUpdated(delta: number): void {
    this.totalTime += delta;
    this.map.onPixiFrameUpdated(delta, this.totalTime);
    this.hud.onPixiFrameUpdated(delta, this.totalTime);
  }

  public resetPlayerPositions(): void {
    this.map.resetPlayerPositions();
  }
}
