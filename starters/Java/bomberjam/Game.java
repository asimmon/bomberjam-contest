package bomberjam;

import com.google.gson.Gson;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class Game {
    private static final Gson GSON = new Gson();

    private final BufferedReader stdin;
    private boolean isReady;
    private String myPlayerId;
    private State state;

    public Game() {
        this.stdin = new BufferedReader(new InputStreamReader(System.in));
        this.isReady = false;
        this.myPlayerId = null;
        this.state = null;
    }

    public String getMyPlayerId() {
        this.ensureIsReady();
        return this.myPlayerId;
    }

    public State getState() {
        this.ensureIsReady();
        this.ensureInitialState();
        return this.state;
    }

    public Player getMyPlayer() {
        this.ensureIsReady();
        this.ensureInitialState();
        return this.state.getPlayers().get(this.myPlayerId);
    }

    public void setReady(String playerName) throws Exception {
        if (this.isReady)
            return;

        if (playerName == null || playerName.length() == 0)
            throw new IllegalArgumentException("Your name cannot be null or empty");

        System.out.println("0:" + playerName);
        System.out.flush();

        this.myPlayerId = stdin.readLine();

        if (!isInteger(this.myPlayerId))
            throw new IllegalStateException("Could not retrieve your ID from standard input");

        this.isReady = true;
    }

    private static boolean isInteger(String text) {
        if (text == null) {
            return false;
        }

        try {
            int i = Integer.parseInt(text);
        } catch (NumberFormatException nfe) {
            return false;
        }

        return true;
    }

    public void receiveCurrentState() throws Exception
    {
        this.ensureIsReady();
        this.state = GSON.fromJson(this.stdin.readLine(), State.class);
    }

    public void sendAction(ActionKind action)
    {
        this.ensureIsReady();
        this.ensureInitialState();

        String actionStr = Constants.ACTION_KIND_TO_ACTION_STRING_MAPPINGS.get(action);

        System.out.println(this.state.getTick() + ":" + actionStr);
        System.out.flush();
    }

    private void ensureIsReady()
    {
        if (!this.isReady)
            throw new IllegalStateException("You need to call Game.ready(...) with your name first");
    }

    private void ensureInitialState()
    {
        if (this.state == null)
            throw new IllegalStateException("You need to call Game.receiveCurrentState() to retrieve the initial state of the game");
    }
}
