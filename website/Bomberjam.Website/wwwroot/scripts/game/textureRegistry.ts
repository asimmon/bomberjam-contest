import { IResourceDictionary, Texture, Spritesheet, Sprite, AnimatedSprite } from 'pixi.js';
import { Sprites } from './assets';

export default class TextureRegistry {
  private readonly spritePools: AnimatedSpritePool;
  private readonly spritesheet: Spritesheet;

  public readonly floor: Texture[];
  public readonly wall: Texture[];
  public readonly block: Texture[];
  public readonly player: {
    front: Texture[];
    back: Texture[];
    left: Texture[];
    right: Texture[];
  };
  public readonly bomb: Texture[];
  public readonly flame: Texture[];
  public readonly fireBonus: Texture[];
  public readonly bombBonus: Texture[];
  public readonly tileSize: number = 48;
  public readonly spriteRatio: number;

  constructor(resources: IResourceDictionary) {
    this.spritePools = new AnimatedSpritePool();
    const spritesheet = resources[Sprites.spritesheet].spritesheet;
    if (!spritesheet) throw new Error('Could not load spritesheet ' + Sprites.spritesheet);
    this.spritesheet = spritesheet;

    for (let id in this.spritesheet.textures) {
      if (this.spritesheet.textures.hasOwnProperty(id)) {
        this.spritesheet.textures[id].defaultAnchor.set(0, 0);
      }
    }

    this.floor = this.spritesheet.animations[Sprites.floor];
    this.wall = this.spritesheet.animations[Sprites.wall];
    this.block = this.spritesheet.animations[Sprites.block];
    this.player = {
      front: this.spritesheet.animations[Sprites.player.front],
      back: this.spritesheet.animations[Sprites.player.back],
      left: this.spritesheet.animations[Sprites.player.left],
      right: this.spritesheet.animations[Sprites.player.right]
    };
    this.bomb = this.spritesheet.animations[Sprites.bomb];
    this.flame = this.spritesheet.animations[Sprites.flame];
    this.fireBonus = this.spritesheet.animations[Sprites.bonuses.flame];
    this.bombBonus = this.spritesheet.animations[Sprites.bonuses.bomb];

    this.spriteRatio = this.tileSize / this.floor[0].width;
  }

  public makeAnimatedSprite(textures: Texture[]): AnimatedSprite {
    return this.spritePools.get(textures);
  }

  public freeAnimatedSprite(sprite: AnimatedSprite): void {
    this.spritePools.free(sprite);
  }

  destroy() {
    this.spritePools.dispose();

    this.floor.forEach(t => t.destroy(true));
    this.wall.forEach(t => t.destroy(true));
    this.block.forEach(t => t.destroy(true));
    this.player.front.forEach(t => t.destroy(true));
    this.player.back.forEach(t => t.destroy(true));
    this.player.left.forEach(t => t.destroy(true));
    this.player.right.forEach(t => t.destroy(true));
    this.bomb.forEach(t => t.destroy(true));
    this.flame.forEach(t => t.destroy(true));
    this.fireBonus.forEach(t => t.destroy(true));
    this.bombBonus.forEach(t => t.destroy(true));

    this.spritesheet.destroy(true);
  }
}

class AnimatedSpritePool {
  private readonly underlyingPools: Map<Texture[], SpritePool<AnimatedSprite>>;

  public constructor() {
    this.underlyingPools = new Map<Texture[], SpritePool<AnimatedSprite>>();
  }

  public get(textures: Texture[]): AnimatedSprite {
    return this.getUnderlyingPool(textures).get();
  }

  public free(sprite: AnimatedSprite): void {
    this.getUnderlyingPool(sprite.textures).free(sprite);
  }

  private getUnderlyingPool(textures: Texture[]): SpritePool<AnimatedSprite> {
    if (!textures || textures.length === 0) throw new Error('Cannot instanciate an animated sprite pool without textures');

    let pool = this.underlyingPools.get(textures);

    if (!pool) {
      pool = new SpritePool(() => new AnimatedSprite(textures, true));
      this.underlyingPools.set(textures, pool);
    }

    return pool;
  }

  public dispose(): void {
    this.underlyingPools.forEach(underlyingPool => underlyingPool.dispose());
    this.underlyingPools.clear();
  }
}

class SpritePool<TSprite extends Sprite> {
  private readonly spriteFactory: () => TSprite;
  private readonly usedSprites: Set<TSprite>;
  private readonly freeSprites: Array<TSprite>;

  public constructor(spriteFactory: () => TSprite) {
    this.spriteFactory = spriteFactory;
    this.usedSprites = new Set<TSprite>();
    this.freeSprites = [];
  }

  public get(): TSprite {
    let sprite = this.freeSprites.shift();
    if (!sprite) sprite = this.spriteFactory();
    this.usedSprites.add(sprite);
    return sprite;
  }

  public free(sprite: TSprite): void {
    if (this.usedSprites.has(sprite)) {
      this.usedSprites.delete(sprite);
      this.freeSprites.push(sprite);
    }
  }

  public dispose(): void {
    this.freeSprites.push(...this.usedSprites);
    this.usedSprites.clear();
    for (const sprite of this.freeSprites) sprite.destroy();
    this.freeSprites.length = 0;
  }
}
