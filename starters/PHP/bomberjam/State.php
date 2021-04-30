<?php

namespace bomberjam;

class State
{
    /** @var int */
    public $tick;

    /** @var bool */
    public $isFinished;

    /** @var string */
    public $tiles;

    /** @var array<string, Player> */
    public $players;

    /** @var array<string, Bomb> */
    public $bombs;

    /** @var array<string, Bonus> */
    public $bonuses;

    /** @var int */
    public $width;

    /** @var int */
    public $height;

    /** @var int */
    public $suddenDeathCountdown;

    /** @var bool */
    public $isSuddenDeathEnabled;

    public function __construct($jsonState)
    {
        $this->tick = $jsonState['tick'];
        $this->isFinished = $jsonState['isFinished'];
        $this->tiles = $jsonState['tiles'];
        $this->width = $jsonState['width'];
        $this->height = $jsonState['height'];
        $this->suddenDeathCountdown = $jsonState['suddenDeathCountdown'];
        $this->isSuddenDeathEnabled = $jsonState['isSuddenDeathEnabled'];

        $this->players = array();
        $this->bombs = array();
        $this->bonuses = array();

        foreach ($jsonState['players'] as $id => $jsonPlayer) {
            $this->players[strval($id)] = new Player($jsonPlayer);
        }

        foreach ($jsonState['bombs'] as $id => $jsonBomb) {
            $this->bombs[strval($id)] = new Bomb($jsonBomb);
        }

        foreach ($jsonState['bonuses'] as $id => $jsonBonus) {
            $this->bonuses[strval($id)] = new Bonus($jsonBonus);
        }
    }

    /**
     * @param $x int
     * @param $y int
     * @return bool
     */
    public function isOutOfBounds($x, $y)
    {
        return $x < 0 || $y < 0 || $x >= $this->width || $y >= $this->height;
    }

    /**
     * @param $x int
     * @param $y int
     * @return int
     */
    public function coordToTileIndex($x, $y)
    {
        return $y * $this->width + $x;
    }

    /**
     * @param $x int
     * @param $y int
     * @return string
     */
    public function getTileAt($x, $y)
    {
        if ($this->isOutOfBounds($x, $y)) {
            return Constants::TILE_OUT_OF_BOUNDS;
        }

        return $this->tiles[$this->coordToTileIndex($x, $y)];
    }

    /**
     * @param $x int
     * @param $y int
     * @return Bomb|null
     */
    public function findActiveBombAt($x, $y)
    {
        /** @var Bomb $bomb */
        foreach ($this->bombs as $id => $bomb) {
            if ($bomb->countdown > 0 && $bomb->x === $x && $bomb->y === $y)
                return $bomb;
        }

        return null;
    }

    /**
     * @param $x int
     * @param $y int
     * @return Bonus|null
     */
    public function findDroppedBonusAt($x, $y)
    {
        /** @var Bonus $bonus */
        foreach ($this->bonuses as $id => $bonus) {
            if ($bonus->x === $x && $bonus->y === $y)
                return $bonus;
        }

        return null;
    }

    /**
     * @param $x int
     * @param $y int
     * @return Player|null
     */
    public function findAlivePlayerAt($x, $y)
    {
        /** @var Player $player */
        foreach ($this->players as $id => $player) {
            if ($player->isAlive && $player->x === $x && $player->y === $y)
                return $player;
        }

        return null;
    }
}