import React, {ChangeEvent, useEffect, useRef, useState} from "react";
import replayGame from "../pixi/replayGame";

interface VisualizerProps {
  gameHistory: IGameHistory | null;
}

export const ViewerWithControls = (props: VisualizerProps) => {
  const canvasContainer = useRef<HTMLDivElement>(null);
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

  const onRangeValueChanged = (event: ChangeEvent<HTMLInputElement>) => replayCtrl?.goToStateIdx(Number(event.target.value));
  const onRangeMouseDown = () => setCanManuallySetStateIdx(false);
  const onRangeMouseUp = () => setCanManuallySetStateIdx(true);

  const increaseSpeed = () => replayCtrl?.increaseSpeed();
  const decreaseSpeed = () => replayCtrl?.decreaseSpeed();
  const pauseOrResumeGame = () => isPlaying ? pauseGame() : resumeGame();

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

  useEffect(() => {
    replayCtrl?.destroy();

    if (props.gameHistory && canvasContainer.current) {
      const loadedGameHistory = props.gameHistory;
      replayGame(canvasContainer.current, loadedGameHistory, onStateChanged).then(newReplayCtrl => {

        setReplayCtrl(newReplayCtrl);
        setMinStateIdx(0);
        setMaxStateIdx(loadedGameHistory.ticks.length - 1);
        setIsStarted(true);
        setIsPlaying(true);
      });
    }
  }, [props.gameHistory]);

  return <div className={isStarted ? '' : 'd-none'}>
    <div ref={canvasContainer} className="canvas"/>

    <div className={isStarted ? 'form mt-2' : 'd-none'}>
      <div className="form-row">
        <div className="col-md-auto">
          <a onClick={pauseOrResumeGame} className="btn btn-primary btn-sm mr-2">
            <span className={isPlaying ? '' : 'd-none'}><i className="fas fa-pause"/></span>
            <span className={isPlaying ? 'd-none' : ''}><i className="fas fa-play"/></span>
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