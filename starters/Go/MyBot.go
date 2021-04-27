// Do not rename the "main" package as the bomberjam backend expects a package named "main"
package main

import (
    "main/bomberjam"
    "math/rand"
    "os"
    "strconv"
    "strings"
    "time"
)

var allActions = []bomberjam.ActionKind{bomberjam.ActionUp, bomberjam.ActionDown, bomberjam.ActionLeft, bomberjam.ActionRight, bomberjam.ActionStay, bomberjam.ActionBomb,}

func main() {
    rand.Seed(time.Now().UnixNano())

    // Standard output (fmt.Println, os.Stdout) can ONLY BE USED to communicate with the bomberjam process
    // Use text files if you need to log something for debugging
    logger := setupLogging()
    defer logger.Close()

    game := bomberjam.NewGame()

    // 1) You must send an alphanumerical name up to 32 characters
    // Spaces or special characters are not allowed
    myPlayerName := "MyName" + strconv.Itoa(rand.Intn(9999))
    if err := game.Ready(myPlayerName); err != nil {
        logger.Error(err.Error())
        panic(err)
    }
    logger.Info("My player ID is " + game.MyPlayerId)

    for isPlaying := true; isPlaying; isPlaying = game.MyPlayer.IsAlive && !game.State.IsFinished {
        if err := game.ReceiveCurrentState(); err != nil {
            logger.Error(err.Error())
            panic(err)
        }

        // 3) Analyze the current state and decide what to do
        for x := 0; x < game.State.Width; x++ {
            for y := 0; y < game.State.Height; y++ {
                if tile := game.State.GetTileAt(x, y); tile == bomberjam.TileBlock {
                    // TODO found a block to destroy
                }

                if otherPlayer := game.State.FindAlivePlayerAt(x, y); otherPlayer != nil && otherPlayer.Id != game.MyPlayerId {
                    // TODO found an alive opponent
                }

                if bomb := game.State.FindActiveBombAt(x, y); bomb != nil {
                    // TODO found an active bomb
                }

                if bonus := game.State.FindDroppedBonusAt(x, y); bonus != nil {
                    // TODO found a bonus
                }
            }
        }

        if game.MyPlayer.BombsLeft > 0 {
            // TODO you can drop a bomb
        }

        // 4) Send your action
        action := allActions[rand.Intn(len(allActions))]
        game.SendAction(action)
        logger.Info("Tick " + strconv.Itoa(game.State.Tick) + ", sent action: " + string(action))
    }
}

func setupLogging() bomberjam.Logger {
    logger := bomberjam.NewLogger()

    // Edit run_game.(bat|sh) to include file logging for any of the four bot processes: go run main --logging
    for _, arg := range os.Args {
        if "--logging" == strings.ToLower(arg) {
            _ = logger.Setup("log-" + strconv.FormatInt(time.Now().Unix(), 10) + ".log")
            break
        }
    }

    return logger
}
