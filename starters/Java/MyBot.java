import bomberjam.*;

import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class MyBot {
    private static final Random RNG = new Random();

    private static final List<ActionKind> ALL_ACTIONS = Arrays.asList(
            ActionKind.UP, ActionKind.DOWN, ActionKind.LEFT, ActionKind.RIGHT, ActionKind.STAY, ActionKind.BOMB
    );

    public static void main(String[] args) throws Exception {
        // Standard output (System.out.println) can ONLY BE USED to communicate with the bomberjam process
        // Use text files if you need to log something for debugging
        Game game = new Game();

        // 1) You must send an alphanumerical name up to 32 characters
        // Spaces or special characters are not allowed
        game.setReady("MyName" + (RNG.nextInt(9999 - 1000) + 1000));

        do {
            // 2) Each tick, you'll receive the current game state serialized as JSON
            // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
            game.receiveCurrentState();

            try {
                // 3) Analyze the current state and decide what to do
                for (var x = 0; x < game.getState().getWidth(); x++) {
                    for (var y = 0; y < game.getState().getHeight(); y++) {
                        var tile = game.getState().getTileAt(x, y);
                        if (tile == TileKind.BLOCK) {
                            // TODO found a block to destroy
                        }

                        Player otherPlayer = game.getState().findAlivePlayerAt(x, y);
                        if (otherPlayer != null && !otherPlayer.getId().equals(game.getMyPlayerId())) {
                            // TODO found an alive opponent
                        }

                        Bomb bomb = game.getState().findActiveBombAt(x, y);
                        if (bomb != null) {
                            // TODO found an active bomb
                        }

                        Bonus bonus = game.getState().findDroppedBonusAt(x, y);
                        if (bonus != null) {
                            // TODO found a bonus
                        }
                    }
                }

                if (game.getMyPlayer().getBombsLeft() > 0) {
                    // TODO you can drop a bomb
                }

                // 4) Send your action
                ActionKind action = ALL_ACTIONS.get(RNG.nextInt(ALL_ACTIONS.size()));
                game.sendAction(action);
            } catch (Exception ex) {
                // Handle your exceptions per tick
            }
        } while (game.getMyPlayer().getIsAlive());
    }
}
