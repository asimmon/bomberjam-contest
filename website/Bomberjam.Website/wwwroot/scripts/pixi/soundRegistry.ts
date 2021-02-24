import { IResourceDictionary, Loader } from 'pixi.js';
import { Musics, Sounds } from './assets';

import sound from 'pixi-sound';

export default class SoundRegistry {
  public readonly level: sound.Sound;
  public readonly waiting: sound.Sound;
  public readonly victory: sound.Sound;

  public readonly coin: sound.Sound;
  public readonly error: sound.Sound;
  public readonly explosion: sound.Sound;
  public readonly footsteps: sound.Sound;
  public readonly powerup: sound.Sound;
  public readonly death: sound.Sound;
  public readonly bomb: sound.Sound;
  public readonly pause: sound.Sound;
  public readonly unpause: sound.Sound;
  public readonly stomp: sound.Sound;

  constructor(resources: IResourceDictionary) {
    this.waiting = resources.waiting.sound;
    this.waiting.loop = true;
    this.waiting.singleInstance = true;

    this.level = resources.level.sound;
    this.level.loop = true;
    this.level.volume = 0.5;
    this.level.singleInstance = true;

    this.victory = resources.victory.sound;
    this.victory.singleInstance = true;

    this.coin = resources.coin.sound;
    this.error = resources.error.sound;
    this.explosion = resources.explosion.sound;
    this.powerup = resources.powerup.sound;
    this.death = resources.death.sound;
    this.bomb = resources.bomb.sound;
    this.pause = resources.pause.sound;
    this.unpause = resources.unpause.sound;
    this.stomp = resources.stomp.sound;

    this.footsteps = resources.footsteps.sound;
    this.footsteps.volume = 0.5;
  }

  public pauseAll() {
    sound.muteAll();
  }

  public resumeAll() {
    sound.unmuteAll();
  }

  static loadResources(loader: Loader) {
    loader.add('level', Musics.level);
    loader.add('waiting', Musics.waiting);
    loader.add('victory', Musics.victory);

    loader.add('coin', Sounds.coin);
    loader.add('error', Sounds.error);
    loader.add('explosion', Sounds.explosion);
    loader.add('footsteps', Sounds.footsteps);
    loader.add('powerup', Sounds.powerup);
    loader.add('death', Sounds.death);
    loader.add('bomb', Sounds.bomb);
    loader.add('pause', Sounds.pause);
    loader.add('unpause', Sounds.unpause);
    loader.add('stomp', Sounds.stomp);
  }

  destroy() {
    sound.stopAll();
    sound.removeAll();
  }
}
