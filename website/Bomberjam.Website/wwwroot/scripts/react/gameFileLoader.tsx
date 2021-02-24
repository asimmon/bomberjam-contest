import React, {ChangeEvent, useEffect} from "react";

interface GameFileLoaderProps {
  gameId: string;
  onLoad: (gameHistory: IGameHistory) => void;
  onError: (error: string) => void;
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

  useEffect(() => {
    if (props.gameId.length > 0) {
      downloadGameHistory(props.gameId).then(gameHistory => {
        props.onLoad(gameHistory);
      }, err => {
        props.onError(err.toString());
      });
    }
  }, [props.gameId]);

  return <div className={props.gameId.length > 0 ? 'd-none' : ''}>
    <p className="lead">Select a replay file</p>
    <div className="custom-file mb-3">
      <input type="file" className="custom-file-input" onChange={onFileChanged} />
      <label className="custom-file-label">Select a *.json file</label>
    </div>
  </div>
};