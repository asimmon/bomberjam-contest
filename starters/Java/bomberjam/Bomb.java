package bomberjam;

import com.google.gson.annotations.SerializedName;

public class Bomb {
    @SerializedName("x")
    private Integer x;

    @SerializedName("y")
    private Integer y;

    @SerializedName("playerId")
    private String playerId;

    @SerializedName("countdown")
    private Integer countdown;

    @SerializedName("range")
    private Integer range;

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

    public String getPlayerId() {
        return playerId;
    }

    public void setPlayerId(String playerId) {
        this.playerId = playerId;
    }

    public Integer getCountdown() {
        return countdown;
    }

    public void setCountdown(Integer countdown) {
        this.countdown = countdown;
    }

    public Integer getRange() {
        return range;
    }

    public void setRange(Integer range) {
        this.range = range;
    }
}