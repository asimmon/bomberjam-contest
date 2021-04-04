package bomberjam;

import java.io.Closeable;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;

public class Logger implements Closeable {
    private PrintWriter file;

    public Logger() {
        this.file = null;
    }

    public void setup(String filename) throws IOException {
        if (this.file == null)
            this.file = new PrintWriter(new FileWriter(filename, false));
    }

    public void debug(String text) {
        this.write("DEBUG", text);
    }

    public void info(String text) {
        this.write("INFO", text);
    }

    public void warn(String text) {
        this.write("WARN", text);
    }

    public void error(String text) {
        this.write("ERROR", text);
    }

    private void write(String level, String text) {
        if (this.file != null) {
            this.file.println(level + ": " + text);
            this.file.flush();
        }
    }

    public void close() {
        if (this.file != null) {
            this.file.close();
            this.file = null;
        }
    }
}
