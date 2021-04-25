package bomberjam

import (
    "log"
    "os"
)

type Logger struct {
    file   *os.File
    logger *log.Logger
}

func NewLogger() Logger {
    return Logger{
        file: nil,
    }
}

func (l *Logger) Setup(filename string) error {
    if l.file == nil {
        f, err := os.OpenFile(filename, os.O_WRONLY|os.O_CREATE|os.O_APPEND, 0644)
        if err != nil {
            return err
        }
        l.file = f
        l.logger = log.New(f, "", log.Lmicroseconds)
    }
    return nil
}

func (l *Logger) write(level string, text string) {
    if l.file != nil {
        l.logger.Printf(level + ": " + text)
    }
}

func (l *Logger) Debug(text string) { l.write("DEBUG", text) }
func (l *Logger) Info(text string)  { l.write("INFO", text) }
func (l *Logger) Warn(text string)  { l.write("WARN", text) }
func (l *Logger) Error(text string) { l.write("ERROR", text) }

func (l *Logger) Close() {
    if l.file != nil {
        _ = l.file.Close()
        l.file = nil
    }
}
