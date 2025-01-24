namespace Guppy.Core.Logging.Common.Constants
{
    public static class LoggingConstants
    {
        public const string SourceContext = nameof(SourceContext);
        public const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}";
    }
}