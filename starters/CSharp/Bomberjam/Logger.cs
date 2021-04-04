using System;
using System.IO;

namespace MyBot.Bomberjam
{
    public class Logger : IDisposable
    {
        private StreamWriter _file;

        public Logger()
        {
            this._file = null;
        }

        public void Setup(string filename)
        {
            if (this._file == null)
                this._file = new StreamWriter(File.OpenWrite(filename));
        }

        public void Debug(string text)
        {
            this.Write("DEBUG", text);
        }

        public void Info(string text)
        {
            this.Write("INFO", text);
        }

        public void Warn(string text)
        {
            this.Write("WARN", text);
        }

        public void Error(string text)
        {
            this.Write("ERROR", text);
        }

        private void Write(string level, string text)
        {
            if (this._file != null)
            {
                this._file.WriteLine(level + ": " + text);
                this._file.Flush();
            }
        }

        public void Close()
        {
            if (this._file != null)
            {
                this._file.Close();
                this._file = null;
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}