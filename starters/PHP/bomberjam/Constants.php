<?php

namespace bomberjam;

class Constants
{
    const ACTION_UP = 'up';
    const ACTION_DOWN = 'down';
    const ACTION_LEFT = 'left';
    const ACTION_RIGHT = 'right';
    const ACTION_STAY = 'stay';
    const ACTION_BOMB = 'bomb';

    const BONUS_BOMB = 'bomb';
    const BONUS_FIRE = 'fire';

    const TILE_EMPTY = '.';
    const TILE_WALL = '#';
    const TILE_BLOCK = '+';
    const TILE_EXPLOSION = '*';
    const TILE_OUT_OF_BOUNDS = '';
}