package bomberjam;

import java.util.HashMap;
import java.util.Map;

public final class Constants {
    public static final String UP = "up";
    public static final String DOWN = "down";
    public static final String LEFT = "left";
    public static final String RIGHT = "right";
    public static final String STAY = "stay";
    public static final String BOMB = "bomb";
    public static final String FIRE = "fire";

    public static final char EMPTY = '.';
    public static final char WALL = '#';
    public static final char BLOCK = '+';
    public static final char EXPLOSION = '*';

    public static final String TOP_LEFT = "tl";
    public static final String TOP_RIGHT = "tr";
    public static final String BOTTOM_LEFT = "bl";
    public static final String BOTTOM_RIGHT = "br";

    public static final Map<ActionKind, String> ACTION_KIND_TO_ACTION_STRING_MAPPINGS = new HashMap<ActionKind, String>() {{
        put(ActionKind.UP, UP);
        put(ActionKind.DOWN, DOWN);
        put(ActionKind.LEFT, LEFT);
        put(ActionKind.RIGHT, RIGHT);
        put(ActionKind.STAY, STAY);
        put(ActionKind.BOMB, BOMB);
    }};

    public static final Map<Character, TileKind> TILE_CHAR_TO_TILE_KIND_MAPPINGS = new HashMap<Character, TileKind>() {{
        put(EMPTY, TileKind.EMPTY);
        put(WALL, TileKind.WALL);
        put(BLOCK, TileKind.BLOCK);
        put(EXPLOSION, TileKind.EXPLOSION);
    }};
}