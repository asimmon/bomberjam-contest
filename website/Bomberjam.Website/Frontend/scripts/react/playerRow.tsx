import React, {useEffect, useState} from "react";

interface PlayerRowProps {
  playerIndex: string;
  gameHistory: IGameHistory;
}

const NA = 'N/A';

const isArray = (obj: any): obj is Array<any> => Array.isArray(obj);
const isObject = (obj: any): obj is Object => typeof obj === 'object';
const isNumber = (obj: any): obj is Number => typeof obj === 'number';
const isString = (obj: any): obj is String => typeof obj === 'string';

const calcAvg = (items: number[]) => items.reduce((acc, item) => acc + item, 0) / items.length;

const calcStdDev = (items: number[], avg: number) => {
  const sqDiffs = items.map(item => {
    const diff = item - avg;
    return diff * diff;
  }, 0);

  const avgSqDiff = sqDiffs.reduce((acc, sqDiff) => acc + sqDiff, 0) / sqDiffs.length;
  return Math.sqrt(avgSqDiff);
};

export const PlayerRow = (props: PlayerRowProps) => {
  const [name, setName] = useState<string>('');
  const [score, setScore] = useState<string>('');
  const [latency, setLatency] = useState<string>('');
  const [timeoutCount, setTimeoutCount] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    try {
      let hasScore = false;
      let hasName = false;

      if (isObject(props.gameHistory.summary) && isObject(props.gameHistory.summary.players)) {
        const playerSummary = props.gameHistory.summary.players[props.playerIndex];
        if (isObject(playerSummary)) {
          if (isNumber(playerSummary.score)) {
            setScore(String(playerSummary.score));
            hasScore = true;
          }

          if (isString(playerSummary.name)) {
            setName(playerSummary.name);
            hasName = true;
          }
        }
      }

      if (!hasScore) {
        setScore(NA);
      }

      if (!hasName) {
        setName(NA);
      }

      let timeoutCount = 0;
      const latencies: number[] = [];

      if (isArray(props.gameHistory.ticks)) {
        for (let tick of props.gameHistory.ticks) {
          if (isObject(tick) && isObject(tick.latencies)) {
            const latency = tick.latencies[props.playerIndex];
            if (isNumber(latency))
              latencies.push(latency);
          }

          if (isObject(tick.state) && isObject(tick.state.players) && isObject(tick.state.players[props.playerIndex]))
            if (tick.state.players[props.playerIndex].timedOut)
              timeoutCount++;
        }
      }

      setTimeoutCount(String(timeoutCount));

      if (latencies.length > 0) {
        const avg = calcAvg(latencies);
        const stdDev = calcStdDev(latencies, avg);

        const roundedAvgMs = Math.round((avg * 1000) * 100) / 100;
        const roundedStdDevMs = Math.round((stdDev * 1000) * 100) / 100;

        setLatency(`${roundedAvgMs} (±${roundedStdDevMs})`);
      } else {
        setLatency(NA);
      }
    } catch (err) {
      setErrorMessage(typeof err === 'string' ? err : err.toString());
      setScore(NA);
      setLatency(NA);
      setTimeoutCount(NA);
    }
  }, [props.gameHistory, props.playerIndex]);

  if (errorMessage.length > 0) {
    return <tr><td colSpan={4}>{errorMessage}</td></tr>;
  }

  return <tr>
    <td>{name}</td>
    <td>{score}</td>
    <td>{latency}</td>
    <td>{timeoutCount}</td>
  </tr>;
};