import { AnimatedSprite, Container, DisplayObject, Sprite, Texture, TilingSprite } from 'pixi.js';
import GameContainer from './gameContainer';
import SoundRegistry from './soundRegistry';
import TextureRegistry from './textureRegistry';

export default class GameMap extends GameContainer {
  private readonly textures: TextureRegistry;
  private readonly sounds: SoundRegistry;
  private readonly mapContainer: Container;

  private wallSprites: { [idx: number]: AnimatedSprite } = {};
  private blockSprites: { [idx: number]: AnimatedSprite } = {};
  private playerSprites: { [playerId: string]: AnimatedSprite } = {};
  private bombSprites: { [bombId: string]: AnimatedSprite } = {};
  private bonusesSprites: { [bonusId: string]: AnimatedSprite } = {};
  private flameSprites: AnimatedSprite[] = [];

  constructor(stateProvider: IStateProvider, textures: TextureRegistry, sounds: SoundRegistry) {
    super(stateProvider);

    this.textures = textures;
    this.sounds = sounds;
    this.mapContainer = new Container();
  }

  public initialize() {
    const containerSize = {
      width: (this.state.width + 2) * this.textures.tileSize,
      height: (this.state.height + 2) * this.textures.tileSize
    };

    const mapSize = {
      width: this.state.width * this.textures.tileSize,
      height: this.state.height * this.textures.tileSize
    };

    const wallTilingSprite = new TilingSprite(this.textures.wall[0], containerSize.width, containerSize.height);
    wallTilingSprite.tileScale.set(this.textures.spriteRatio, this.textures.spriteRatio);

    this.container.addChild(wallTilingSprite);

    const floorTilingSprite = new TilingSprite(this.textures.floor[0], mapSize.width, mapSize.height);
    floorTilingSprite.tileScale.set(this.textures.spriteRatio, this.textures.spriteRatio);

    const floorTilingSpriteContainer = new Container();
    floorTilingSpriteContainer.position.set(this.textures.tileSize, this.textures.tileSize);
    floorTilingSpriteContainer.addChild(floorTilingSprite);

    this.container.addChild(floorTilingSpriteContainer);

    this.mapContainer.position.set(this.textures.tileSize, this.textures.tileSize);

    this.container.addChild(this.mapContainer);
  }

  onPlayerAdded(playerId: string, player: IPlayer): void {
    this.registerPlayer(playerId, player);
    this.sounds.coin.play();
  }

  onPlayerRemoved(playerId: string, player: IPlayer): void {
    this.unregisterObjectSprite(this.playerSprites, playerId);
    this.sounds.error.play();
  }

  onBombAdded(bombId: string, bomb: IBomb): void {
    this.registerBomb(bombId, bomb);
    this.sounds.bomb.play();
  }

  onBombRemoved(bombId: string, bomb: IBomb): void {
    this.unregisterObjectSprite(this.bombSprites, bombId);
  }

  onBonusAdded(bonusId: string, bonus: IBonus): void {
    this.registerBonus(bonusId, bonus);
  }

  onBonusRemoved(bonusId: string, bonus: IBonus): void {
    this.unregisterObjectSprite(this.bonusesSprites, bonusId);

    // If player got Bonus
    for (const playerId in this.state.players) {
      const player: IPlayer = this.state.players[playerId];
      if (bonus.x === player.x && bonus.y === player.y) {
        this.sounds.powerup.play();
      }
    }
  }

  public onStateChanged(prevState: IGameState) {
    for (const playerId in this.playerSprites) {
      const oldPlayer = prevState.players[playerId];
      const newPlayer = this.state.players[playerId];

      if (oldPlayer && newPlayer) {
        const hasJustRespawned = newPlayer.respawning === this.stateProvider.configuration.respawnTime - 1;

        if (hasJustRespawned || newPlayer.y > oldPlayer.y) {
          this.unregisterObjectSprite(this.playerSprites, playerId);
          this.registerPlayer(playerId, oldPlayer, 'front');
        } else if (newPlayer.x > oldPlayer.x) {
          this.unregisterObjectSprite(this.playerSprites, playerId);
          this.registerPlayer(playerId, oldPlayer, 'right');
        } else if (newPlayer.x < oldPlayer.x) {
          this.unregisterObjectSprite(this.playerSprites, playerId);
          this.registerPlayer(playerId, oldPlayer, 'left');
        } else if (newPlayer.y < oldPlayer.y) {
          this.unregisterObjectSprite(this.playerSprites, playerId);
          this.registerPlayer(playerId, oldPlayer, 'back');
        } else {
          this.playerSprites[playerId].stop();
        }

        const sprite: Sprite = this.playerSprites[playerId];

        // On replay mode, you can skip multiple ticks so we don't want to animate players in that case
        const hasSwitchManyTicksInReplayMode = Math.abs(this.state.tick - prevState.tick) > 1;
        const gameEnded = this.state.isFinished;

        if (gameEnded || hasSwitchManyTicksInReplayMode || hasJustRespawned) {
          sprite.x = newPlayer.x * this.textures.tileSize;
          sprite.y = newPlayer.y * this.textures.tileSize;

          sprite.vx = 0;
          sprite.vy = 0;
        } else {
          sprite.x = oldPlayer.x * this.textures.tileSize;
          sprite.y = oldPlayer.y * this.textures.tileSize;

          sprite.vx = oldPlayer.x === newPlayer.x ? 0 : newPlayer.x - oldPlayer.x > 0 ? this.textures.tileSize : -this.textures.tileSize;
          sprite.vy = oldPlayer.y === newPlayer.y ? 0 : newPlayer.y - oldPlayer.y > 0 ? this.textures.tileSize : -this.textures.tileSize;
        }
      }
    }

    // Game started
    if (prevState.tick === 1 && this.state.tick > 1) {
      this.sounds.waiting.stop();
      this.sounds.level.play();
    }
    // Game ended
    else if (!prevState.isFinished && this.state.isFinished) {
      this.sounds.level.stop();
      this.sounds.victory.play();
    }
    // Game was ended but the replay started the game again
    else if (prevState.isFinished && !this.state.isFinished) {
      this.sounds.victory.stop();
      this.sounds.level.play();
    }

    // Hide bombs that just exploded
    for (const bombId in this.state.bombs) {
      const bomb: IBomb = this.state.bombs[bombId];
      const bombSprite: Sprite = this.bombSprites[bombId];

      if (bombSprite) {
        if (bomb.countdown <= 0) {
          bombSprite.visible = false;
          this.sounds.explosion.play();
        } else {
          bombSprite.visible = true;
        }
      }
    }

    for (const playerId in this.state.players) {
      const newPlayer: IPlayer = this.state.players[playerId];
      const oldPlayer: IPlayer = prevState.players[playerId];
      const playerSprite: Sprite = this.playerSprites[playerId];

      if (playerSprite) {
        // Hide dead players
        if (!newPlayer.isAlive && !newPlayer.hasWon) {
          playerSprite.visible = false;
        } else {
          playerSprite.visible = true;
        }

        if (
          (oldPlayer && oldPlayer.isAlive !== newPlayer.isAlive) ||
          newPlayer.respawning === this.stateProvider.configuration.respawnTime
        ) {
          this.sounds.death.play();
        }

        // Restore player transparency if not respawning
        if (newPlayer.isAlive && newPlayer.respawning === 0) {
          playerSprite.alpha = 1;
        }
      }
    }

    this.displayFlames();

    const previousWallCount = Object.keys(this.wallSprites).length;
    this.displayWallsAndBlocks();
    const currentWallCount = Object.keys(this.wallSprites).length;

    // New wall has dropped.
    if (previousWallCount !== currentWallCount) this.sounds.stomp.play();
  }

  private fixZOrdering(): void {
    this.mapContainer.children.sort((s1: DisplayObject, s2: DisplayObject) => {
      const y1 = Math.floor(s1.y / this.textures.tileSize);
      const y2 = Math.floor(s2.y / this.textures.tileSize);
      return y1 - y2;
    });
  }

  public onPixiFrameUpdated(delta: number, totalTime: number): void {
    if (typeof this.state.tickDuration !== 'number' || this.state.tickDuration <= 0)
      throw new Error('Expected positive state tick duration');

    // round total time to 100ms and blink if it can be divided by 200
    const normalizedTotalTime = Math.round(totalTime / 100) * 100;
    const shouldBlink = normalizedTotalTime % 200 === 0;
    const progress = delta / this.state.tickDuration;

    for (const playerId in this.playerSprites) {
      const sprite: Sprite = this.playerSprites[playerId];
      const player: IPlayer = this.state.players[playerId];

      sprite.x += sprite.vx * progress;
      sprite.y += sprite.vy * progress;

      if (player.isAlive && player.respawning > 0 && player.respawning < this.stateProvider.configuration.respawnTime) {
        sprite.alpha = shouldBlink ? 0.4 : 1;
      }
    }

    // z-ordering
    this.fixZOrdering();
  }

  private registerPlayer(playerId: string, player: IPlayer, orientation: 'left' | 'right' | 'front' | 'back' = 'front'): void {
    let textures: Texture[] = this.textures.player.front;
    if (orientation === 'left') textures = this.textures.player.left;
    else if (orientation === 'right') textures = this.textures.player.right;
    else if (orientation === 'front') textures = this.textures.player.front;
    else if (orientation === 'back') textures = this.textures.player.back;

    const sprite = this.makeAnimatedSprite(textures, player, false, 0.15);
    sprite.anchor.set(0, 0.5);
    sprite.tint = player.color;
    this.playerSprites[playerId] = sprite;
    this.mapContainer.addChild(sprite);
    this.sounds.footsteps.play();
  }

  private registerBomb(bombId: string, bomb: IBomb) {
    if (bomb.countdown >= 0) {
      const sprite = this.makeAnimatedSprite(this.textures.bomb, bomb, true, 0.15);
      sprite.anchor.set(0.5, 0.5);
      this.bombSprites[bombId] = sprite;
      this.mapContainer.addChild(sprite);
    }
  }

  private registerBonus(bonusId: string, bonus: IBonus) {
    const texture = bonus.kind === 'bomb' ? this.textures.bombBonus : this.textures.fireBonus;
    const sprite = this.makeAnimatedSprite(texture, bonus, true, 0.15);
    sprite.anchor.set(0.5, 0.5);
    this.bonusesSprites[bonusId] = sprite;
    this.mapContainer.addChild(sprite);
  }

  private displayFlames() {
    for (const sprite of this.flameSprites) {
      this.unregisterSprite(sprite);
    }

    this.flameSprites.length = 0;

    for (let x = 0; x < this.state.width; x++) {
      for (let y = 0; y < this.state.height; y++) {
        const idx = y * this.state.width + x;
        const char = this.state.tiles[idx];

        if (char === '*') {
          const sprite = this.makeAnimatedSprite(this.textures.flame, { x: x, y: y }, true, 0.15);
          sprite.anchor.set(0.5, 0.5);
          this.mapContainer.addChild(sprite);
          this.flameSprites.push(sprite);
        }
      }
    }
  }

  private displayWallsAndBlocks() {
    let currentBlockFrame = 0;
    let currentWallFrame = 0;

    for (const idx in this.blockSprites) {
      currentBlockFrame = this.blockSprites[idx].currentFrame + 1;
      break;
    }

    for (const idx in this.wallSprites) {
      currentWallFrame = this.wallSprites[idx].currentFrame + 1;
      break;
    }

    for (let x = 0; x < this.state.width; x++) {
      for (let y = 0; y < this.state.height; y++) {
        const idx = y * this.state.width + x;
        const char = this.state.tiles[idx];

        let blockSprite: AnimatedSprite = this.blockSprites[idx];
        let wallSprite: AnimatedSprite = this.wallSprites[idx];

        if (char === '+') {
          if (wallSprite) {
            this.unregisterSprite(wallSprite);
            delete this.wallSprites[idx];
          }

          if (!blockSprite) {
            blockSprite = this.makeAnimatedSprite(this.textures.block, { x: x, y: y }, false, 0.15, currentBlockFrame);
            this.blockSprites[idx] = blockSprite;
            this.mapContainer.addChild(blockSprite);
          }
        } else if (char === '#') {
          if (blockSprite) {
            this.unregisterSprite(blockSprite);
            delete this.blockSprites[idx];
          }

          if (!wallSprite) {
            wallSprite = this.makeAnimatedSprite(this.textures.wall, { x: x, y: y }, false, 0.15, currentWallFrame);
            this.wallSprites[idx] = wallSprite;
            this.mapContainer.addChild(wallSprite);
          }
        } else {
          if (blockSprite) {
            this.unregisterSprite(blockSprite);
            delete this.blockSprites[idx];
          }

          if (wallSprite) {
            this.unregisterSprite(wallSprite);
            delete this.wallSprites[idx];
          }
        }
      }
    }
  }

  private unregisterObjectSprite(sprites: { [k: string]: AnimatedSprite }, key: string) {
    const sprite = sprites[key];
    if (sprite) {
      this.unregisterSprite(sprite);
      delete sprites[key];
    }
  }

  private unregisterSprite(sprite: AnimatedSprite) {
    this.mapContainer.removeChild(sprite);
    this.textures.freeAnimatedSprite(sprite);
  }

  private makeAnimatedSprite(
    textures: Texture[],
    pos: IHasPos,
    centered: boolean,
    speed: number,
    startingFrame: number = 0
  ): AnimatedSprite {
    const sprite = this.textures.makeAnimatedSprite(textures);

    if (centered) {
      sprite.position.set(
        pos.x * this.textures.tileSize + this.textures.tileSize / 2.0,
        pos.y * this.textures.tileSize + this.textures.tileSize / 2.0
      );
    } else {
      sprite.position.set(pos.x * this.textures.tileSize, pos.y * this.textures.tileSize);
    }

    sprite.animationSpeed = speed;
    sprite.scale.set(this.textures.spriteRatio, this.textures.spriteRatio);
    sprite.vx = 0;
    sprite.vy = 0;

    sprite.gotoAndPlay(startingFrame);

    return sprite;
  }

  public resetPlayerPositions() {
    for (const playerId in this.playerSprites) {
      const player = this.state.players[playerId];
      const sprite: Sprite = this.playerSprites[playerId];

      sprite.x = player.x * this.textures.tileSize;
      sprite.y = player.y * this.textures.tileSize;
      sprite.vx = 0;
      sprite.vy = 0;
    }

    this.fixZOrdering();
  }
}
