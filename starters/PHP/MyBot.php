<?php

use bomberjam\Constants;
use bomberjam\Game;
use bomberjam\Logger;

spl_autoload_register(function ($className) {
    require str_replace('\\', DIRECTORY_SEPARATOR, ltrim($className, '\\')) . '.php';
});

$allActions = [
    Constants::ACTION_UP,
    Constants::ACTION_RIGHT,
    Constants::ACTION_DOWN,
    Constants::ACTION_LEFT,
    Constants::ACTION_STAY,
    Constants::ACTION_BOMB
];

$allObstacles = [
	"endGrid" => 1,
	"destroyable_block" => 2,
	"undestroyable_block" => 3,
	"exploding_bomb" => 4,
	"bomb" => 5,
	"opponent" => 6 
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
$game->ready('0:Alberto03' . rand(1000, 9999));
$logger->info('My player ID is ' . $game->myPlayerId);

$logger->info('my position ' . $game->myPlayer->x.' '.$game->myPlayer->y);

do {
    // 2) Each tick, you'll receive the current game state serialized as JSON
    // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    $game->receiveCurrentState();
	
    try {
		$map = [];
		$bombs = [];
		$opponents = [];
		$bonuses = [];
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
					$opponents[$x][$y] = 1;
                }

                $bomb = $game->state->findActiveBombAt($x, $y);
                if (!is_null($bomb)) {
                    // TODO found an active bomb
					$bombs[$x][$y] = 1;
                }

                $bonus = $game->state->findDroppedBonusAt($x, $y);
                if (!is_null($bonus)) {
                    // TODO found a bonus
					$bonuses[$x][$y] = 1;
                }
				
				$map[$x][$y] = $tile;
				
            }
        }
		
		$myPositionX = $game->myPlayer->x;
		$myPositionY = $game->myPlayer->y;
		
		$oneUp = $myPositionY - 1;
		$oneDown = $myPositionY + 1;
		$oneLeft = $myPositionX - 1;
		$oneRight = $myPositionX + 1;
		
		$typeOfObstacle = [];
		$moveAvailable = [];

		//check four directions
		//up
		$moveAvailable[0] = 0;
		if(!empty($map[$myPositionX][$oneUp]))
		{
			for($up = $oneUp; $up >= 0; $up--)
			{
				$elementInPositionToEvaluate = $map[$myPositionX][$up];
				
				$isABomb = $bombs[$myPositionX][$up] === 1; //!empty();
				$isAnOpponent = !empty($opponents[$myPositionX][$up]);
				$isABonus = !empty($bonuses[$myPositionX][$up]);
				
				$typeOfObstacle[0] = getTypeOfObstacle($elementInPositionToEvaluate, $isABomb, $isAnOpponent);
				
				if(!empty($typeOfObstacle[0])){
					break;
				}
				
				$moveAvailable[0]++;
			}
			if(empty($typeOfObstacle[0])){
				$typeOfObstacle[0] = 1; //end grid
			}
		}
		
		//right
		$moveAvailable[1] = 0;
	//	$typeOfObstacle[1] = null;
		if(!empty( $map[$oneRight][$myPositionY]))
		{
			for($right = $oneRight; $right < $game->state->width; $right++)
			{
				$elementInPositionToEvaluate = $map[$right][$myPositionY];
				
				$isABomb = !empty($bombs[$right][$myPositionY]);
				$isAnOpponent = !empty($opponents[$right][$myPositionY]);
				$isABonus = !empty($bonuses[$right][$myPositionY]);
				
				$typeOfObstacle[1] = getTypeOfObstacle($elementInPositionToEvaluate, $isABomb, $isAnOpponent);
				
				if(!empty($typeOfObstacle[1])){
					break;
				}
				
				$moveAvailable[1]++;
			}
			if(empty($typeOfObstacle[1])){
				$typeOfObstacle[1] = 1; //end grid
			}
		}
	
		//down
		$moveAvailable[2] = 0;
	//	$typeOfObstacle[2] = null;
		if(!empty($map[$myPositionX][$oneDown]))
		{
			for($down = $oneDown; $down < $game->state->height; $down++)
			{
				$elementInPositionToEvaluate = $map[$myPositionX][$down];
				
				$isABomb = !empty($bombs[$myPositionX][$down]);
				$isAnOpponent = !empty($opponents[$myPositionX][$down]);
				$isABonus = !empty($bonuses[$myPositionX][$down]);
				
				$typeOfObstacle[2] = getTypeOfObstacle($elementInPositionToEvaluate, $isABomb, $isAnOpponent);
				
				if(!empty($typeOfObstacle[2])){
					break;
				}
				
				$moveAvailable[2]++;
			}
			if(empty($typeOfObstacle[2])){
				$typeOfObstacle[2] = 1; //end grid
			}
		}
		
		//left
		$moveAvailable[3] = 0;
	//	$typeOfObstacle[3] = null;
		if(!empty($map[$oneLeft][$myPositionY]))
		{
			for($left = $oneLeft; $left >= 0; $left--)
			{
				$elementInPositionToEvaluate = $map[$left][$myPositionY];
				
				$isABomb = !empty($bombs[$left][$myPositionY]);
				$isAnOpponent = !empty($opponents[$left][$myPositionY]);
				$isABonus = !empty($bonuses[$left][$myPositionY]);
				
				$typeOfObstacle[3] = getTypeOfObstacle($elementInPositionToEvaluate, $isABomb, $isAnOpponent);
				
				if(!empty($typeOfObstacle[3])){
					break;
				}
				
				$moveAvailable[3]++;
			}
			if(empty($typeOfObstacle[3])){
				$typeOfObstacle[3] = 1; //end grid
			}
		}

		//$logger->info('bombs '. print_r($game->state->bombs, true));

		$action = null;
		if(in_array($allObstacles["bomb"], $typeOfObstacle) || $game->state->findActiveBombAt($myPositionX, $myPositionY)) //bomb discovered
		{
			$logger->info('bomb?');
			$action = runFromBomb($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $game, $logger);
		}
		elseif(in_array($allObstacles["opponent"], $typeOfObstacle)) //opponent discovered 
		{
			$logger->info('opponent?');
			$action = huntOpponent($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger);
		}
		elseif(in_array($allObstacles["destroyable_block"], $typeOfObstacle)) //destroyable block discovered
		{
			$logger->info('destroyable block?');
			$action = destroyBlock($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger);
		}
		else{ //just free path
			$logger->info('random?');
			$action = runRandom($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger);
		}

		$logger->info('moveAvailable up ' . $moveAvailable[0]);
		$logger->info('moveAvailable right ' . $moveAvailable[1]);
		$logger->info('moveAvailable down ' . $moveAvailable[2]);
		$logger->info('moveAvailable left ' . $moveAvailable[3]);
		$logger->info('typeOfObstacle ' . print_r($typeOfObstacle, true));
		//$logger->info('moveAvailable ' . print_r($moveAvailable, true));
		

		
        if ($game->myPlayer->bombsLeft > 0) {
            // TODO you can drop a bomb
        }
        
		// 4) Send your action
		$action = !empty($action) ? $action : $allActions[array_rand($allActions)];
	//	$logger->info('action: ' . print_r($action));

        $game->sendAction($action);
        $logger->info('Tick ' . $game->state->tick . ', sent ' . $action);
    } catch (Exception $ex) {
        // Handle your exceptions per tick
        $logger->error('Tick ' . $game->state->tick . ', exception: ' . $ex->getTraceAsString());
    }

} while ($game->myPlayer->isAlive && !$game->state->isFinished);


function getTypeOfObstacle($elementInPositionToEvaluate, $isABomb, $isAnOpponent)
{
	$typeOfObstacle = null;
	//end of grid or obstacle
	if(	$elementInPositionToEvaluate === Constants::TILE_OUT_OF_BOUNDS ){   //end grid
		$typeOfObstacle = 1;
	}
	if(	$elementInPositionToEvaluate === Constants::TILE_BLOCK ){   //destroyable block
		$typeOfObstacle = 2;
	}
	if(	$elementInPositionToEvaluate === Constants::TILE_WALL ){   //undestroyable block
		$typeOfObstacle = 3;
	}
	if(	$elementInPositionToEvaluate === Constants::TILE_EXPLOSION ){  //exploding bomb
		$typeOfObstacle = 4;
	}
	if(	$isABomb ){   
		$typeOfObstacle = 5;
	}
	if(	$isAnOpponent ){  
		$typeOfObstacle = 6;
	}
	return $typeOfObstacle;
}

function runFromBomb($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $game, $logger)
{
	$obstacleDirections = array_keys($typeOfObstacle, $allObstacles["bomb"]);
	

	$bombDirection = $obstacleDirections[0];

	unset($moveAvailable[$bombDirection]);
	asort($moveAvailable);
	end($moveAvailable);
	$directionMove = key($moveAvailable);

	//move away from bomb 
	$action = $allActions[$directionMove];
	return $action;
}
function huntOpponent($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger)
{
	$obstacleDirections = array_keys($typeOfObstacle, $allObstacles["opponent"]);
	$direction = $obstacleDirections[0];
	
//	$logger->info('opponent');

	if($moveAvailable[$direction] > 2)
	{
		//move toward 
		$action = $allActions[$direction];
	}
	else{
		//drop bomb
		$action = $allActions[5];
	}
	
	return $action;
}
function destroyBlock($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger)
{
	$obstacleDirections = array_keys($typeOfObstacle, $allObstacles["destroyable_block"]);
	
	$direction = $obstacleDirections[0];
	
//	$logger->info('block to destroy');

	if($moveAvailable[$direction] > 0)
	{
		//move toward 
		$action = $allActions[$direction];
	}
	else{
		//drop bomb
		$action = $allActions[5];
	}
	
	return $action;
}
function runRandom($moveAvailable, $typeOfObstacle, $allActions, $allObstacles, $logger)
{
//	$logger->info('runRandom');

	//drop bomb
	$action = $allActions[5];
	
	return $action;
}