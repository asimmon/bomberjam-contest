import * as angular from 'angular';
import replayGame from "./game/game";

export default class GameVisualizerController {
  private readonly timeoutService: angular.ITimeoutService;

  private replayCtrl: IReplayGameController | null = null;
  private canChangeRangeValue: boolean = true;

  public stateJsonStr: string = '';
  public replayStarted: boolean = false;

  public minStateIdx: number = 0;
  public maxStateIdx: number = 10;
  public selectedStateIdx: number = 0;
  public isPlaying: boolean = false;

  public constructor(timeoutService: angular.ITimeoutService) {
    this.timeoutService = timeoutService;

    const gameFilePicker = document.getElementById<HTMLInputElement>('game-file-picker');
    if (!gameFilePicker) {
      throw new Error('Could not find game file picker');
    }

    gameFilePicker.addEventListener('change', async () => {
      if (gameFilePicker.files && gameFilePicker.files.length > 0 && gameFilePicker.files[0].name.toUpperCase().endsWith('.JSON')) {
        const selectedFile = gameFilePicker.files[0];
        await this.loadGameHistoryFile(selectedFile);
      }
    }, false);
  }

  private async loadGameHistoryFile(file: File): Promise<void> {
    const fileContents = await this.readFileAsText(file);
    const history = await this.parseGameHistoryFromText(fileContents);
    await this.loadGameHistory(history);
  }

  private readFileAsText(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      try {
        const reader = new FileReader();

        reader.onload = () => {
          if (reader.readyState === FileReader.DONE && typeof reader.result === 'string') {
            resolve(reader.result);
          }
        };

        reader.readAsText(file);
      } catch (err) {
        reject(err);
      }
    });
  }

  private parseGameHistoryFromText(text: string): Promise<IGameHistory> {
    return new Promise<IGameHistory>((resolve, reject) => {
      try {
        const history = JSON.parse<IGameHistory>(text);

        // TODO handle invalid content
        resolve(history);
      } catch (err) {
        reject(err);
      }
    });
  }

  private async loadGameHistory(history: IGameHistory): Promise<void> {
    const onStateChanged = (stateIdx: number, newState: IGameState) => {
      if (this.canChangeRangeValue) {
        this.selectedStateIdx = stateIdx;
      }
      this.stateJsonStr = JSON.stringify(newState, null, 2);
    }

    this.replayCtrl = await replayGame(history, onStateChanged, this.timeoutService);

    this.minStateIdx = 0;
    this.maxStateIdx = history.ticks.length - 1;
    this.replayStarted = true;
    this.isPlaying = true;
  }

  public pauseOrResumeGame() {
    if (this.isPlaying) {
      this.pauseGame();
    } else {
      this.resumeGame();
    }
  }

  private resumeGame() {
    if (this.replayCtrl) {
      this.replayCtrl.resumeGame();
      this.isPlaying = true;
    }
  }

  private pauseGame() {
    if (this.replayCtrl) {
      this.replayCtrl.pauseGame();
      this.isPlaying = false;
    }
  }

  public increaseSpeed() {
    this.replayCtrl?.increaseSpeed();
  }

  public decreaseSpeed() {
    this.replayCtrl?.decreaseSpeed();
  }

  public onSelectedTickChanged() {
    this.replayCtrl?.goToStateIdx(this.selectedStateIdx);
  }

  public tickRangeMouseDown() {
    this.canChangeRangeValue = false;
  }

  public tickRangeMouseUp() {
    this.canChangeRangeValue = true;
  }
}