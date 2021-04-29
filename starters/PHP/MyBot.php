<?php

use bomberjam\Constants;
use bomberjam\Game;
use bomberjam\Logger;

spl_autoload_register(function ($className) {
    require str_replace('\\', DIRECTORY_SEPARATOR, ltrim($className, '\\')) . '.php';
});

$allActions = [
    Constants::ACTION_UP,
    Constants::ACTION_DOWN,
    Constants::ACTION_LEFT,
    Constants::ACTION_RIGHT,
    Constants::ACTION_STAY,
    Constants::ACTION_BOMB
];

// Standard output (print) can ONLY BE USED to communicate with the bomberjam process
// Use text files if you need to log something for debugging
$logger = new Logger();

if (isset($argv)) {
    foreach ($argv as $arg) {
        // Edit run_game.(bat|sh) to include file logging for any of the four bot processes: php MyBot.php --logging
        if ('--logging' === strtolower($arg)) {
            $logger->setup('log-' . round(microtime(true) * 1000) . '.log');
            break;
        }
    }
}

$game = new Game();

// 1) You must send an alphanumerical name up to 32 characters
// Spaces or special characters are not allowed
$game->ready('MyName' . rand(1000, 9999));
$logger->info('My player ID is ' . $game->myPlayerId);

do {
    // 2) Each tick, you'll receive the current game state serialized as JSON
    // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    $game->receiveCurrentState();

    try {
        // 3) Analyze the current state and decide what to do
        for ($x = 0; $x < $game->state->width; $x++) {
            for ($y = 0; $y < $game->state->height; $y++) {
                $tile = $game->state->getTileAt($x, $y);
                if ($tile === Constants::TILE_BLOCK) {
                    // TODO found a block to destroy
                }

                $otherPlayer = $game->state->findAlivePlayerAt($x, $y);
                if (!is_null($otherPlayer) && $otherPlayer->id !== $game->myPlayerId) {
                    // TODO found an alive opponent
                }

                $bomb = $game->state->findActiveBombAt($x, $y);
                if (!is_null($bomb)) {
                    // TODO found an active bomb
                }

                $bonus = $game->state->findDroppedBonusAt($x, $y);
                if (!is_null($bonus)) {
                    // TODO found a bonus
                }
            }
        }

        if ($game->myPlayer->bombsLeft > 0) {
            // TODO you can drop a bomb
        }

        // 4) Send your action
        $action = $allActions[array_rand($allActions)];
        $game->sendAction($action);
        $logger->info('Tick ' . $game->state->tick . ', sent ' . $action);
    } catch (Exception $ex) {
        // Handle your exceptions per tick
        $logger->error('Tick ' . $game->state->tick . ', exception: ' . $ex->getTraceAsString());
    }

} while ($game->myPlayer->isAlive && !$game->state->isFinished);