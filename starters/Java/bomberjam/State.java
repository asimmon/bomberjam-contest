package bomberjam;

import com.google.gson.annotations.SerializedName;

import java.util.Map;

public class State {
    @SerializedName("tiles")
    private String tiles;

    @SerializedName("tick")
    private Integer tick;

    @SerializedName("isFinished")
    private Boolean isFinished;

    @SerializedName("players")
    private Map<String, Player> players;

    @SerializedName("bombs")
    private Map<String, Bomb> bombs;

    @SerializedName("bonuses")
    private Map<String, Bonus> bonuses;

    @SerializedName("width")
    private Integer width;

    @SerializedName("height")
    private Integer height;

    @SerializedName("suddenDeathCountdown")
    private Integer suddenDeathCountdown;

    @SerializedName("isSuddenDeathEnabled")
    private Boolean isSuddenDeathEnabled;

    public String getTiles() {
        return tiles;
    }

    public void setTiles(String tiles) {
        this.tiles = tiles;
    }

    public Integer getTick() {
        return tick;
    }

    public void setTick(Integer tick) {
        this.tick = tick;
    }

    public Boolean isFinished() {
        return isFinished;
    }

    public void setIsFinished(Boolean isFinished) {
        this.isFinished = isFinished;
    }

    public Map<String, Player> getPlayers() {
        return players;
    }

    public void setPlayers(Map<String, Player> players) {
        this.players = players;
    }

    public Map<String, Bomb> getBombs() {
        return bombs;
    }

    public void setBombs(Map<String, Bomb> bombs) {
        this.bombs = bombs;
    }

    public Map<String, Bonus> getBonuses() {
        return bonuses;
    }

    public void setBonuses(Map<String, Bonus> bonuses) {
        this.bonuses = bonuses;
    }

    public Integer getWidth() {
        return width;
    }

    public void setWidth(Integer width) {
        this.width = width;
    }

    public Integer getHeight() {
        return height;
    }

    public void setHeight(Integer height) {
        this.height = height;
    }

    public Integer getSuddenDeathCountdown() {
        return suddenDeathCountdown;
    }

    public void setSuddenDeathCountdown(Integer suddenDeathCountdown) {
        this.suddenDeathCountdown = suddenDeathCountdown;
    }

    public Boolean isSuddenDeathEnabled() {
        return isSuddenDeathEnabled;
    }

    public void setIsSuddenDeathEnabled(Boolean isSuddenDeathEnabled) {
        this.isSuddenDeathEnabled = isSuddenDeathEnabled;
    }

    public TileKind getTileAt(int x, int y) {
        if (this.isOutOfBounds(x, y))
            return TileKind.OUT_OF_BOUNDS;

        char tileChar = this.tiles.charAt(this.coordToTileIndex(x, y));
        return Constants.TILE_CHAR_TO_TILE_KIND_MAPPINGS.get(tileChar);
    }

    private boolean isOutOfBounds(int x, int y) {
        return x < 0 || y < 0 || x >= this.width || y >= this.height;
    }

    private int coordToTileIndex(int x, int y) {
        return y * this.width + x;
    }

    public Bomb findActiveBombAt(int x, int y) {
        for (Bomb b : this.bombs.values())
            if (b.getCountdown() > 0 && b.getX() == x && b.getY() == y)
                return b;

        return null;
    }

    public Bonus findDroppedBonusAt(int x, int y) {
        for (Bonus b : this.bonuses.values())
            if (b.getX() == x && b.getY() == y)
                return b;

        return null;
    }

    public Player findAlivePlayerAt(int x, int y) {
        for (Player p : this.players.values())
            if (p.isAlive() && p.getX() == x && p.getY() == y)
                return p;

        return null;
    }
}