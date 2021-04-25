package bomberjam

type TileKind byte

const (
    TileOutOfBounds TileKind = 0
    TileEmpty       TileKind = '.'
    TileWall        TileKind = '#'
    TileBlock       TileKind = '+'
    TileExplosion   TileKind = '*'
)

type ActionKind string

const (
    ActionUp    ActionKind = "up"
    ActionDown  ActionKind = "down"
    ActionLeft  ActionKind = "left"
    ActionRight ActionKind = "right"
    ActionStay  ActionKind = "stay"
    ActionBomb  ActionKind = "bomb"
)
