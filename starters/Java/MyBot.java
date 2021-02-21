import com.google.gson.*;
import java.io.*;
import java.util.*;

public class MyBot {
    private static final List<String> ALL_ACTIONS = Arrays.asList("up", "down", "left", "right", "stay", "bomb");
    private static final Random RNG = new Random();
    private static final Gson JSON = new Gson();

    public static void main(String[] args) throws Exception {
        // Standard output (System.out.println) can ONLY BE USED to communicate with the bomberjam process
        // Use text files if you need to log something for debugging

        try (BufferedReader stdin = new BufferedReader(new InputStreamReader(System.in))) {
            // 1) You must send an alphanumerical name up to 32 characters, prefixed by "0:"
            // No spaces or special characters are allowed
            System.out.println("0:MyName" + (RNG.nextInt(9999 - 1000) + 1000));
            System.out.flush();

            // 2) Receive your player ID from the standard input
            String playerId = stdin.readLine();

            while (true) {
                // 3) Each tick, you'll receive the current game state serialized as JSON
                // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
                JsonObject state = JSON.fromJson(stdin.readLine(), JsonObject.class);

                try {
                    // 4) Send your action prefixed by the current tick number and a colon
                    String randomAction = ALL_ACTIONS.get(RNG.nextInt(ALL_ACTIONS.size()));
                    System.out.println(state.get("tick").getAsInt() + ":" + randomAction);
                    System.out.flush();
                } catch (Exception ex) {
                    // Handle your exceptions per tick
                }
            }
        }
    }
}
