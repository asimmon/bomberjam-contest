package bomberjam

type State struct {
    Tiles                string
    Tick                 int
    IsFinished           bool
    Players              map[string]Player
    Bombs                map[string]Bomb
    Bonuses              map[string]Bonus
    Width                int
    Height               int
    SuddenDeathCountdown int
    IsSuddenDeathEnabled bool
}

func (s *State) IsOutOfBounds(x int, y int) bool {
    return x < 0 || y < 0 || x >= s.Width || y >= s.Height
}

func (s *State) CoordToTileIndex(x int, y int) int {
    return y*s.Width + x
}

func (s *State) GetTileAt(x int, y int) TileKind {
    if s.IsOutOfBounds(x, y) {
        return TileOutOfBounds
    }

    tileChar := s.Tiles[s.CoordToTileIndex(x, y)]
    return TileKind(tileChar)
}

func (s *State) FindActiveBombAt(x int, y int) *Bomb {
    for _, b := range s.Bombs {
        if b.Countdown > 0 && b.X == x && b.Y == y {
            return &b
        }
    }

    return nil
}

func (s *State) FindAlivePlayerAt(x int, y int) *Player {
    for _, p := range s.Players {
        if p.IsAlive && p.X == x && p.Y == y {
            return &p
        }
    }

    return nil
}

func (s *State) FindDroppedBonusAt(x int, y int) *Bonus {
    for _, b := range s.Bonuses {
        if b.X == x && b.Y == y {
            return &b
        }
    }

    return nil
}
