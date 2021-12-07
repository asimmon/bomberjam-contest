namespace Bomberjam.Website.Configuration
{
    public sealed class TelemetryOptions
    {
        public string GoogleAnalytics { get; set; } = string.Empty;

        public string Hotjar { get; set; } = string.Empty;
    }
}