import React, {useState} from "react";
import {GameFileLoader} from "./gameFileLoader";
import {ViewerWithControls} from "./viewerWithControls";
import {PlayerTable} from "./playerTable";

interface ApplicationProps {
  gameId: string;
}

export const Application = (props: ApplicationProps) => {
  const [loadingText, setLoadingText] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [gameHistory, setGameHistory] = useState<IGameHistory | null>(null);

  const onLoading = (loadingText: string) => {
    setErrorMessage('');
    setLoadingText(loadingText);
  };

  const loadGameHistory = async (newGameHistory: IGameHistory): Promise<void> => {
    setGameHistory(newGameHistory);
    setErrorMessage('');
    setLoadingText('');
  };

  const showLoadingError = (error: string): void => {
    setErrorMessage(error);
    setLoadingText('');
  };

  return <div>
    <div className={errorMessage.length ? 'alert alert-danger mb-2' : 'd-none'}>{errorMessage}</div>
    <GameFileLoader gameId={props.gameId} onLoading={onLoading} onLoaded={loadGameHistory} onError={showLoadingError}/>
    <ViewerWithControls gameHistory={gameHistory} loadingText={loadingText} />
    <PlayerTable gameHistory={gameHistory} />
  </div>;
};