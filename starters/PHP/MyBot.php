<?php

use bomberjam\Game;
use bomberjam\Constants;

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

$game = new Game();

// 1) You must send an alphanumerical name up to 32 characters
// Spaces or special characters are not allowed
$game->ready('MyName' . rand(1000, 9999));

do {
    $game->receiveCurrentState();

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

} while ($game->myPlayer->isAlive && !$game->state->isFinished);