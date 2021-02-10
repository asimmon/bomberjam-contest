import React, {ChangeEvent, useEffect, useState} from "react";
import replayGame from "./game/game";

interface VisualizerProps {
  gameId: string;
}

export const Visualizer = (props: VisualizerProps) => {
  const [minStateIdx, setMinStateIdx] = useState(0);
  const [maxStateIdx, setMaxStateIdx] = useState(1);
  const [selectedStateIdx, setSelectedStateIdx] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const [isStarted, setIsStarted] = useState(false);
  const [canManuallySetStateIdx, setCanManuallySetStateIdx] = useState(true);
  const [replayCtrl, setReplayCtrl] = useState<IReplayGameController | null>(null);

  const downloadGameHistory = async (gameId: string): Promise<IGameHistory> => {
    const response = await fetch('/api/game/' + gameId);
    if (response.ok) return response.json();
    throw new Error('API returned: ' + response.status + ' ' + response.statusText);
  }

  const downloadAndLoadGameHistory = async (gameId: string): Promise<void> => {
    const history = await downloadGameHistory(gameId);
    await loadGameHistory(history);
  }

  const loadGameHistoryFile = async (file: File): Promise<void> => {
    const fileContents = await readFileAsText(file);
    const history = await parseGameHistoryFromText(fileContents);
    await loadGameHistory(history);
  }

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
  }

  const parseGameHistoryFromText = (text: string): Promise<IGameHistory> => {
    return new Promise<IGameHistory>((resolve, reject) => {
      try {
        resolve(JSON.parse<IGameHistory>(text));
      } catch (err) {
        reject(err);
      }
    });
  }

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

  const onFileChanged = (event: ChangeEvent<HTMLInputElement>): void => {
    if (event.target.files && event.target.files.length && event.target.files[0] && event.target.files[0].name.toUpperCase().endsWith('.JSON')) {
      const maxUploadSize = 2097152;
      const file = event.target.files[0];
      if (file.size && file.size <= maxUploadSize) {
        const _ = loadGameHistoryFile(file);
      }
    }
  };

  if (props.gameId.length > 0) {
    useEffect(() => {
      downloadAndLoadGameHistory(props.gameId).catch(err => {
        // TODO display error
        console.log(err);
      });
    }, []);
  }

  return <div>
    <div className={props.gameId.length > 0 ? 'd-none' : ''}>
      <input type="file" onChange={onFileChanged} />
    </div>

    <div id="canvas" />

    <div className={isStarted ? '' : 'd-none'}>
      <div>
        <input
          type="range"
          min={minStateIdx}
          max={maxStateIdx}
          value={selectedStateIdx}
          onChange={onRangeValueChanged}
          onMouseDown={onRangeMouseDown}
          onMouseUp={onRangeMouseUp}
        />

        <span>{selectedStateIdx} / {maxStateIdx}</span>
      </div>

      <div>
        <button onClick={pauseOrResumeGame} className="btn btn-primary btn-sm">
          <span className={isPlaying ? '' : 'd-none'}><i className="fas fa-pause" /></span>
          <span className={isPlaying ? 'd-none' : ''}><i className="fas fa-play" /></span>
        </button>

        <button onClick={decreaseSpeed} className="btn btn-primary btn-sm">Slower</button>
        <button onClick={increaseSpeed} className="btn btn-primary btn-sm">Faster</button>
      </div>
    </div>
  </div>;
}