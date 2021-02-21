namespace Bomberjam
{
    internal sealed class ProcessMessage
    {
        public ProcessMessage(int? tick, string message)
        {
            this.Tick = tick;
            this.Message = message;
        }

        public ProcessMessage(string message)
            : this(null, message)
        {
        }

        public int? Tick { get; }

        public string Message { get; }
    }
}