package bomberjam

import (
    "bufio"
    "encoding/json"
    "errors"
    "fmt"
    "os"
    "strconv"
    "strings"
)

type Game struct {
    stdin      *bufio.Reader
    isReady    bool
    MyPlayerId string
    MyPlayer   *Player
    State      *State
}

func NewGame() Game {
    return Game{
        stdin:      bufio.NewReader(os.Stdin),
        MyPlayerId: "",
        MyPlayer:   nil,
        State:      nil,
    }
}

func (g *Game) writeLine(text string) {
    fmt.Println(text)
}

func (g *Game) readLine() string {
    text, _ := g.stdin.ReadString('\n')
    return strings.TrimSpace(text)
}

func (g *Game) Ready(playerName string) error {
    if g.isReady {
        return nil
    }

    if len(strings.TrimSpace(playerName)) == 0 {
        return errors.New("your name cannot be null or empty")
    }

    g.writeLine("0:" + playerName)
    myPlayerId := g.readLine()

    if _, err := strconv.Atoi(myPlayerId); err != nil {
        return errors.New("could not retrieve your ID from standard input: '" + myPlayerId + "'")
    }

    g.MyPlayerId = myPlayerId
    g.isReady = true
    return nil
}

func (g *Game) ReceiveCurrentState() error {
    var state State
    if err := json.Unmarshal([]byte(g.readLine()), &state); err != nil {
        return err
    }

    g.State = &state
    myPlayer := state.Players[g.MyPlayerId]
    g.MyPlayer = &myPlayer
    return nil
}

func (g *Game) SendAction(action ActionKind) {
    tickStr := strconv.Itoa(g.State.Tick)
    g.writeLine(tickStr + ":" + string(action))
}
