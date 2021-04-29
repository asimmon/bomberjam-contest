<?php

namespace bomberjam;

class Player
{
    /** @var int */
    public $x;

    /** @var int */
    public $y;

    /** @var string */
    public $id;

    /** @var int */
    public $bombsLeft;

    /** @var int */
    public $maxBombs;

    /** @var int */
    public $bombRange;

    /** @var bool */
    public $isAlive;

    /** @var bool */
    public $timedOut;

    /** @var int */
    public $score;

    /** @var int */
    public $color;

    /** @var int */
    public $respawning;

    /** @var string */
    public $name;

    /** @var string */
    public $startingCorner;

    public function __construct($jsonPlayer)
    {
        $this->x = $jsonPlayer['x'];
        $this->y = $jsonPlayer['y'];
        $this->id = $jsonPlayer['id'];
        $this->bombsLeft = $jsonPlayer['bombsLeft'];
        $this->maxBombs = $jsonPlayer['maxBombs'];
        $this->bombRange = $jsonPlayer['bombRange'];
        $this->isAlive = $jsonPlayer['isAlive'];
        $this->timedOut = $jsonPlayer['timedOut'];
        $this->respawning = $jsonPlayer['respawning'];
        $this->score = $jsonPlayer['score'];
        $this->color = $jsonPlayer['color'];
        $this->name = $jsonPlayer['name'];
        $this->startingCorner = $jsonPlayer['startingCorner'];
    }
}