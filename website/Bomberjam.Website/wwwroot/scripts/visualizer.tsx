import React, {ChangeEvent, useEffect, useState} from "react";
import replayGame from "./game/game";

interface VisualizerProps {
  gameId: string;
}

interface GameFileLoaderProps {
  gameId: string;
  onLoad: (gameHistory: IGameHistory) => void;
  onError: (reason: string) => void;
}

export const GameFileLoader = (props: GameFileLoaderProps) => {
  const readFileAsText = (file: File): Promise<string> => {
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
  };

  const parseGameHistoryFromText = (text: string): Promise<IGameHistory> => {
    return new Promise<IGameHistory>((resolve, reject) => {
      try {
        // TODO validate that some keys exists (ticks, etc.)
        resolve(JSON.parse<IGameHistory>(text));
      } catch (err) {
        reject(err);
      }
    });
  };

  const downloadGameHistory = async (gameId: string): Promise<IGameHistory> => {
    const response = await fetch('/api/game/' + gameId);
    if (response.ok) return response.json();
    throw new Error('API returned: ' + response.status + ' ' + response.statusText);
  };

  const onFileChanged = async (event: ChangeEvent<HTMLInputElement>): Promise<void> => {
    if (!event.target.files || event.target.files.length === 0 || !event.target.files[0])
      return props.onError('No file selected');

    const file = event.target.files[0];
    if (!file.name.toUpperCase().endsWith('.JSON'))
      return props.onError('Please choose a file with a .json extension');

    const maxUploadSize = 2097152;
    if (!file.size || file.size > maxUploadSize)
      return props.onError('File is either empty or greater than 2MB');

    try {
      const fileContents = await readFileAsText(file);
      const gameHistory = await parseGameHistoryFromText(fileContents);
      return props.onLoad(gameHistory);
    } catch (err) {
      return props.onError(err.toString());
    }
  };

  if (props.gameId.length > 0) {
    useEffect(() => {
      downloadGameHistory(props.gameId).then(gameHistory => {
        props.onLoad(gameHistory);
      }, err => {
        props.onError(err.toString());
      });
    }, []);
  }

  return <div className={props.gameId.length > 0 ? 'd-none' : ''}>
    <p className="lead">Select a replay file</p>
    <div className="custom-file mb-3">
      <input type="file" className="custom-file-input" onChange={onFileChanged} />
      <label className="custom-file-label">Select a *.json file</label>
    </div>
  </div>
};

export const Visualizer = (props: VisualizerProps) => {
  const [minStateIdx, setMinStateIdx] = useState(0);
  const [maxStateIdx, setMaxStateIdx] = useState(1);
  const [selectedStateIdx, setSelectedStateIdx] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const [isStarted, setIsStarted] = useState(false);
  const [canManuallySetStateIdx, setCanManuallySetStateIdx] = useState(true);
  const [replayCtrl, setReplayCtrl] = useState<IReplayGameController | null>(null);

  const onStateChanged = (stateIdx: number): void => {
    if (canManuallySetStateIdx) {
      setSelectedStateIdx(stateIdx);
    }
  };

  const loadGameHistory = async (history: IGameHistory): Promise<void> => {
    if (replayCtrl) {
      replayCtrl.destroy();
      setReplayCtrl(null);
      setIsStarted(false);
      setIsPlaying(false);
    }

    const ctrl = await replayGame(history, onStateChanged, window.setTimeout);

    setReplayCtrl(ctrl);
    setMinStateIdx(0);
    setMaxStateIdx(history.ticks.length - 1);
    setIsStarted(true);
    setIsPlaying(true);
  };

  const onRangeValueChanged = (event: ChangeEvent<HTMLInputElement>) => replayCtrl?.goToStateIdx(Number(event.target.value));
  const onRangeMouseDown = () => setCanManuallySetStateIdx(false);
  const onRangeMouseUp = () => setCanManuallySetStateIdx(true);

  const pauseOrResumeGame = () => {
    if (isPlaying) {
      pauseGame();
    } else {
      resumeGame();
    }
  };

  const resumeGame = () => {
    if (replayCtrl) {
      replayCtrl.resumeGame();
      setIsPlaying(true);
    }
  };

  const pauseGame = () => {
    if (replayCtrl) {
      replayCtrl.pauseGame();
      setIsPlaying(false);
    }
  };

  const increaseSpeed = () => replayCtrl?.increaseSpeed();
  const decreaseSpeed = () => replayCtrl?.decreaseSpeed();

  const showLoadingError = (reason: string): void => {
    alert(reason);
  };

  return <div>
    <div className={props.gameId.length > 0 ? 'd-none' : ''}>
      <GameFileLoader gameId={props.gameId} onLoad={loadGameHistory} onError={showLoadingError} />
    </div>

    <div id="canvas" className={isStarted ? 'border rounded' : 'd-none'} />

    <div className={isStarted ? 'for mt-2' : 'd-none'}>
      <div className="form-row">
        <div className="col-md-auto">
          <a onClick={pauseOrResumeGame} className="btn btn-primary btn-sm mr-2">
            <span className={isPlaying ? '' : 'd-none'}><i className="fas fa-pause" /></span>
            <span className={isPlaying ? 'd-none' : ''}><i className="fas fa-play" /></span>
          </a>
          <div className="btn-group btn-group-sm" role="group">
            <a onClick={decreaseSpeed} className="btn btn-primary">Slower</a>
            <a onClick={increaseSpeed} className="btn btn-primary">Faster</a>
          </div>
        </div>
        <div className="col">
          <input
            type="range"
            className="custom-range"
            min={minStateIdx}
            max={maxStateIdx}
            value={selectedStateIdx}
            onChange={onRangeValueChanged}
            onMouseDown={onRangeMouseDown}
            onMouseUp={onRangeMouseUp}
          />
        </div>
      </div>
    </div>
  </div>;
};