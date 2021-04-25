import bomberjam.*;

import java.util.Arrays;
import java.util.Date;
import java.util.List;
import java.util.Random;

public class MyBot {
    private static final Random RNG = new Random();

    private static final List<ActionKind> ALL_ACTIONS = Arrays.asList(
            ActionKind.UP, ActionKind.DOWN, ActionKind.LEFT, ActionKind.RIGHT, ActionKind.STAY, ActionKind.BOMB
    );

    public static void main(String[] args) throws Exception {
        Game game = new Game();

        // Standard output (System.out.println) can ONLY BE USED to communicate with the bomberjam process
        // Use text files if you need to log something for debugging
        try (Logger logger = new Logger()) {
            // Edit run_game.(bat|sh) to include file logging for any of the four bot processes: java -cp "".;*"" MyBot --logging
            for (String arg : args) {
                if ("--logging".equals(arg.toLowerCase())) {
                    logger.setup("log-" + new Date().getTime() + ".log");
                    break;
                }
            }

            // 1) You must send an alphanumerical name up to 32 characters
            // Spaces or special characters are not allowed
            game.setReady("MyName" + (RNG.nextInt(9999 - 1000) + 1000));
            logger.info("My player ID is " + game.getMyPlayerId());

            do {
                // 2) Each tick, you'll receive the current game state serialized as JSON
                // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
                game.receiveCurrentState();

                try {
                    // 3) Analyze the current state and decide what to do
                    for (int x = 0; x < game.getState().getWidth(); x++) {
                        for (int y = 0; y < game.getState().getHeight(); y++) {
                            TileKind tile = game.getState().getTileAt(x, y);
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
                    logger.info("Tick " + game.getState().getTick() + ", sent action: " + action);
                } catch (Exception ex) {
                    // Handle your exceptions per tick
                    logger.error("Tick " + game.getState().getTick() + ", exception: " + ex.toString());
                }
            } while (game.getMyPlayer().isAlive() && !game.getState().isFinished());
        }
    }
}
