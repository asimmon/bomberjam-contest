import React, {useState} from "react";

interface PlayerRowProps {
  playerIndex: string;
  gameHistory: IGameHistory;
}

export const PlayerRow = (props: PlayerRowProps) => {
  const [player, setPlayer] = useState<IPlayerSummary>(props.gameHistory.summary.players[props.playerIndex])
  return <tr>
    <td>{player.name}</td>
  </tr>
};