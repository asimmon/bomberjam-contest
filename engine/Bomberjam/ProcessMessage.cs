namespace Bomberjam
{
    internal sealed class ProcessMessage
    {
        public ProcessMessage(int tick, string message)
        {
            this.Tick = tick;
            this.Message = message;
        }

        public int Tick { get; }

        public string Message { get; }
    }
}