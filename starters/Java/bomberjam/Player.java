package bomberjam;

import com.google.gson.annotations.SerializedName;

public class Player {
    @SerializedName("id")
    private String id;

    @SerializedName("name")
    private String name;

    @SerializedName("x")
    private Integer x;

    @SerializedName("y")
    private Integer y;

    @SerializedName("startingCorner")
    private String startingCorner;

    @SerializedName("bombsLeft")
    private Integer bombsLeft;

    @SerializedName("maxBombs")
    private Integer maxBombs;

    @SerializedName("bombRange")
    private Integer bombRange;

    @SerializedName("isAlive")
    private Boolean isAlive;

    @SerializedName("timedOut")
    private Boolean timedOut;

    @SerializedName("respawning")
    private Integer respawning;

    @SerializedName("score")
    private Integer score;

    @SerializedName("color")
    private Integer color;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Integer getX() {
        return x;
    }

    public void setX(Integer x) {
        this.x = x;
    }

    public Integer getY() {
        return y;
    }

    public void setY(Integer y) {
        this.y = y;
    }

    public String getStartingCorner() {
        return startingCorner;
    }

    public void setStartingCorner(String startingCorner) {
        this.startingCorner = startingCorner;
    }

    public Integer getBombsLeft() {
        return bombsLeft;
    }

    public void setBombsLeft(Integer bombsLeft) {
        this.bombsLeft = bombsLeft;
    }

    public Integer getMaxBombs() {
        return maxBombs;
    }

    public void setMaxBombs(Integer maxBombs) {
        this.maxBombs = maxBombs;
    }

    public Integer getBombRange() {
        return bombRange;
    }

    public void setBombRange(Integer bombRange) {
        this.bombRange = bombRange;
    }

    public Boolean isAlive() {
        return isAlive;
    }

    public void setIsAlive(Boolean isAlive) {
        this.isAlive = isAlive;
    }

    public Boolean getTimedOut() {
        return timedOut;
    }

    public void setTimedOut(Boolean timedOut) {
        this.timedOut = timedOut;
    }

    public Integer getRespawning() {
        return respawning;
    }

    public void setRespawning(Integer respawning) {
        this.respawning = respawning;
    }

    public Integer getScore() {
        return score;
    }

    public void setScore(Integer score) {
        this.score = score;
    }

    public Integer getColor() {
        return color;
    }

    public void setColor(Integer color) {
        this.color = color;
    }

}