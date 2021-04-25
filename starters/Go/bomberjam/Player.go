package bomberjam

type Player struct {
    Id             string
    Name           string
    X              int
    Y              int
    StartingCorner string
    BombsLeft      int
    MaxBombs       int
    BombRange      int
    IsAlive        bool
    IsTimedOut     bool
    Respawning     int
    Score          int
    Color          int
}
