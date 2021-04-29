<?php

namespace bomberjam;

class Bomb
{
    /** @var int */
    public $x;

    /** @var int */
    public $y;

    /** @var string */
    public $playerId;

    /** @var int */
    public $countdown;

    /** @var int */
    public $range;

    public function __construct($jsonBomb)
    {
        $this->x = $jsonBomb['x'];
        $this->y = $jsonBomb['y'];
        $this->playerId = $jsonBomb['playerId'];
        $this->countdown = $jsonBomb['countdown'];
        $this->range = $jsonBomb['range'];
    }
}