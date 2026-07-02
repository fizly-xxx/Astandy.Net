namespace Astandy.Configuration
{
    public class StandClientOptions
    {
        public string? ProxyHost { get; set; }
        public int? ProxyPort { get; set; }
        public string? ProxyUsername { get; set; }
        public string? ProxyPassword { get; set; }
        public int MaxRetryCount { get; set; } = 3;
        public int PingTimeout { get; set; } = 15;
        public bool ReconnectEnable { get; set; } = true;
    }
}