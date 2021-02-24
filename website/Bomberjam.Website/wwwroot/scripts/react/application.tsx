import React, {useState} from "react";
import {GameFileLoader} from "./gameFileLoader";
import {ViewerWithControls} from "./viewerWithControls";
import {PlayerTable} from "./playerTable";

interface ApplicationProps {
  gameId: string;
}

export const Application = (props: ApplicationProps) => {
  const [gameHistory, setGameHistory] = useState<IGameHistory | null>(null);

  const loadGameHistory = async (newGameHistory: IGameHistory): Promise<void> => {
    setGameHistory(newGameHistory);
  };

  const showLoadingError = (error: string): void => {
    alert(error);
  };

  return <div>
    <GameFileLoader gameId={props.gameId} onLoad={loadGameHistory} onError={showLoadingError}/>
    <ViewerWithControls gameHistory={gameHistory} />
    <PlayerTable gameHistory={gameHistory} />
  </div>;
};