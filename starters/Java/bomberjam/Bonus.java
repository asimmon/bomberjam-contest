package bomberjam;

import com.google.gson.annotations.SerializedName;

public class Bonus {
    @SerializedName("x")
    private Integer x;

    @SerializedName("y")
    private Integer y;

    @SerializedName("kind")
    private String kind;

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

    public String getKind() {
        return kind;
    }

    public void setKind(String kind) {
        this.kind = kind;
    }
}