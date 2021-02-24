import React, {useEffect, useState} from "react";
import {PlayerRow} from "./playerRow";

interface PlayerStatisticsProps {
  gameHistory: IGameHistory | null;
}

export const PlayerTable = (props: PlayerStatisticsProps) => {
  const [playerIndexes, setPlayerIndexes] = useState<string[]>([]);

  useEffect(() => {
    if (props.gameHistory) {
      setPlayerIndexes(Object.keys(props.gameHistory.summary.players));
    }
  }, [props.gameHistory]);

  const playerRows = playerIndexes.map(idx =>
    <PlayerRow key={idx} playerIndex={idx} gameHistory={props.gameHistory!} />
  );

  return <div className="mt-4">
    <div className="table-responsive rounded">
      <table className="table table-sm table-bordered table-striped">
        <thead>
        <tr>
          <th>Name</th>
        </tr>
        </thead>
        <tbody>{playerRows}</tbody>
        <tfoot className={playerIndexes.length ? 'd-none' : ''}>
          <tr>
            <td className="text-center">Player statistics will be shown here</td>
          </tr>
        </tfoot>
      </table>
    </div>
  </div>
};