<?php

namespace bomberjam;

class Bonus
{
    /** @var int */
    public $x;

    /** @var int */
    public $y;

    /** @var string */
    public $kind;

    public function __construct($jsonBonus)
    {
        $this->x = $jsonBonus['x'];
        $this->y = $jsonBonus['y'];
        $this->kind = $jsonBonus['kind'];
    }
}