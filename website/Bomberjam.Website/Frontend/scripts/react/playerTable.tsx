import React, {useEffect, useState} from "react";
import {PlayerRow} from "./playerRow";

interface PlayerStatisticsProps {
  gameHistory: IGameHistory | null;
}

export const PlayerTable = (props: PlayerStatisticsProps) => {
  const [playerIndexes, setPlayerIndexes] = useState<string[]>([]);

  useEffect(() => {
    if (props.gameHistory && props.gameHistory.summary && typeof props.gameHistory.summary === 'object') {
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
          <th>Player</th>
          <th>Final score</th>
          <th>Average latency (ms)</th>
          <th><abbr title="How many times this player didn't send an action on time">Timeouts</abbr></th>
        </tr>
        </thead>
        <tbody>{playerRows}</tbody>
        <tfoot className={playerIndexes.length ? 'd-none' : ''}>
          <tr>
            <td className="text-center" colSpan={4}>Player statistics will be shown here</td>
          </tr>
        </tfoot>
      </table>
    </div>
  </div>
};