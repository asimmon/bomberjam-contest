import { Container, Graphics, Sprite, Text, TextStyle, Texture } from 'pixi.js';
import GameContainer from './gameContainer';
import TextureRegistry from './textureRegistry';

interface IPlayerHudContainer extends Container {
  playerSprite: Sprite;
  playerNameText: Text;
  bombSprite: Sprite;
  bombCountText: Text;
  flameSprite: Sprite;
  wifiOffSprite: Sprite;
  bombRangeText: Text;
  scoreText: Text;
}

export default class GameHud extends GameContainer {
  private static readonly TextStyle = new TextStyle({
    fontFamily: 'Arial',
    fontSize: 22
  });

  private readonly textures: TextureRegistry;
  private readonly playerHuds: { [playerId: string]: IPlayerHudContainer };
  private readonly playerBars: { [playerId: string]: Graphics };

  private get canvasHeight(): number {
    return (this.state.height + 2) * this.textures.tileSize;
  }

  private get playerScoreSum(): number {
    let sum = 0;
    for (const playerId in this.state.players) sum += this.state.players[playerId].score;
    return sum;
  }

  constructor(stateProvider: IStateProvider, textures: TextureRegistry) {
    super(stateProvider);

    this.textures = textures;
    this.playerHuds = {};
    this.playerBars = {};
  }

  initialize(): void {
    this.reserveSpace();
    this.refresh();
  }

  onStateChanged(prevState: IGameState): void {
    this.refresh();
  }

  private reserveSpace() {
    const padding = new Graphics();

    padding.beginFill(0xffffff);
    padding.drawRect(0, 0, 350, this.canvasHeight);
    padding.endFill();

    this.container.addChild(padding);
  }

  private refresh() {
    let playerPosition = 0;
    for (const playerId in this.state.players) {
      this.updatePlayerHud(playerPosition, playerId, this.state.players[playerId]);
      playerPosition++;
    }

    this.updateScoreBars();
  }

  private updatePlayerHud(playerPosition: number, playerId: string, player: IPlayer): void {
    if (!this.playerHuds[playerId]) {
      this.addPlayerHud(playerId, player);
    }

    const hud = this.playerHuds[playerId];

    hud.playerSprite.x = 20;
    hud.playerSprite.y = 25;
    hud.playerSprite.tint = player.color;

    hud.playerNameText.x = hud.playerSprite.x + hud.playerSprite.width + 10;
    hud.playerNameText.y = hud.playerSprite.y + 26;

    hud.bombSprite.x = hud.playerNameText.x;
    hud.bombSprite.y = hud.playerNameText.y + hud.playerNameText.height + 5;

    hud.bombCountText.text = player.bombsLeft + '/' + player.maxBombs;
    hud.bombCountText.x = hud.bombSprite.x + hud.bombSprite.width + 5;
    hud.bombCountText.y = hud.bombSprite.y + 6;

    hud.flameSprite.x = hud.bombCountText.x + hud.bombCountText.width + 10;
    hud.flameSprite.y = hud.bombSprite.y;

    hud.wifiOffSprite.x = hud.playerSprite.x + ((hud.playerSprite.width - hud.wifiOffSprite.width) / 2);
    hud.wifiOffSprite.y = hud.playerSprite.y + 40;

    hud.bombRangeText.text = player.bombRange.toString();
    hud.bombRangeText.x = hud.flameSprite.x + hud.flameSprite.width + 5;
    hud.bombRangeText.y = hud.bombSprite.y + 6;

    hud.scoreText.text = `- score: ${player.score.toString()}`;
    hud.scoreText.x = hud.bombRangeText.x + hud.bombRangeText.width + 5;
    hud.scoreText.y = hud.bombSprite.y + 6;

    hud.y = playerPosition * (hud.height + 25);
    hud.alpha = player.isAlive ? 1 : 0.5;

    if (player.isAlive) {
      hud.alpha = 1;

      if (player.timedOut) {
        hud.playerSprite.alpha = 0.5;
        hud.wifiOffSprite.alpha = 1;
      } else {
        hud.playerSprite.alpha = 1;
        hud.wifiOffSprite.alpha = 0;
      }
    } else {
      hud.playerSprite.alpha = 1;
      hud.alpha = 0.5;
    }
  }

  private addPlayerHud(playerId: string, player: IPlayer): void {
    const hud = new Container() as IPlayerHudContainer;

    hud.playerSprite = this.makeStaticSprite(this.textures.player.front[0]);
    hud.playerNameText = new Text(player.name, GameHud.TextStyle);
    hud.bombSprite = this.makeStaticSprite(this.textures.bomb[0]);
    hud.bombCountText = new Text(player.bombsLeft + '/' + player.maxBombs, GameHud.TextStyle);
    hud.flameSprite = this.makeStaticSprite(this.textures.flame[0]);
    hud.wifiOffSprite = this.makeStaticSprite(this.textures.wifiOff[0]);
    hud.bombRangeText = new Text(player.bombRange.toString(), GameHud.TextStyle);
    hud.scoreText = new Text(`- score: ${player.score.toString()}`, GameHud.TextStyle);

    hud.addChild(
      hud.playerSprite,
      hud.playerNameText,
      hud.bombSprite,
      hud.bombCountText,
      hud.flameSprite,
      hud.wifiOffSprite,
      hud.bombRangeText,
      hud.scoreText
    );

    this.playerHuds[playerId] = hud;
    this.container.addChild(hud);
  }

  private makeStaticSprite(texture: Texture): Sprite {
    const sprite = new Sprite(texture);
    sprite.scale.set(this.textures.spriteRatio, this.textures.spriteRatio);
    return sprite;
  }

  private updateScoreBars() {
    const playerScoreSum = this.playerScoreSum;
    const canvasHeight = this.canvasHeight;
    const playerCount = Object.keys(this.state.players).length;
    const playerScoreRatios: { [id: string]: number } = {};

    let extraRatios = 0;

    for (const playerId in this.state.players) {
      // when all scores are equal to 0, there is a tie, so the ratio is 1 / playerCount
      let ratio = playerScoreSum > 0 ? this.state.players[playerId].score / playerScoreSum : 1 / playerCount;
      if (ratio === 0) {
        // at least show a tiny score bar for this player
        ratio = 0.01;
        extraRatios += ratio;
      }

      playerScoreRatios[playerId] = ratio;
    }

    let lastRatioBarY = 0;

    for (const playerId in this.state.players) {
      playerScoreRatios[playerId] = playerScoreRatios[playerId] - extraRatios / playerCount;

      if (!this.playerBars[playerId]) {
        this.playerBars[playerId] = new Graphics();
        this.container.addChild(this.playerBars[playerId]);
      }

      const ratioBar = this.playerBars[playerId];
      const ratioBarHeight = playerScoreRatios[playerId] * canvasHeight;
      const ratioBarWidth = 10;

      ratioBar.clear();
      ratioBar.beginFill(this.state.players[playerId].color);
      ratioBar.drawRect(0, lastRatioBarY, ratioBarWidth, ratioBarHeight);
      ratioBar.endFill();

      lastRatioBarY += ratioBarHeight;
    }
  }
}
