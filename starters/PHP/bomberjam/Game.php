<?php

namespace bomberjam;

use Exception;

class Game
{
    /** @var bool */
    private $isReady;

    /** @var string */
    public $myPlayerId;

    /** @var Player */
    public $myPlayer;

    /** @var State */
    public $state;

    public function __construct()
    {
        $this->isReady = false;
        $this->myPlayerId = null;
        $this->state = null;
    }

    /**
     * @param $playerName string
     * @throws Exception
     */
    public function ready($playerName)
    {
        if ($this->isReady)
            return;

        if (!isset($playerName) || trim($playerName) === '')
            throw new Exception('Your name cannot be null or empty');

        $this->stdout('0:' . $playerName);

        if (($this->myPlayerId = $this->stdin()) === false || !is_numeric($this->myPlayerId))
            throw new Exception('Could not retrieve your ID from standard input');

        $this->isReady = true;
    }

    /**
     * @throws Exception
     */
    public function receiveCurrentState()
    {
        $jsonStateStr = $this->stdin();
        if ($jsonStateStr === false)
            throw new Exception('Could not retrieve the current state from standard input');

        $jsonState = json_decode($jsonStateStr, true, 512, JSON_THROW_ON_ERROR);
        $this->state = new State($jsonState);

        $this->myPlayer = $this->state->players[$this->myPlayerId];
    }

    /**
     * @param $action string
     */
    public function sendAction($action)
    {
        $this->stdout($this->state->tick . ':' . $action);
    }

    private function stdin()
    {
        return (($text = fgets(STDIN)) === false) ? false : rtrim($text);
    }

    private function stdout($text)
    {
        fwrite(STDOUT, $text . PHP_EOL);
    }
}