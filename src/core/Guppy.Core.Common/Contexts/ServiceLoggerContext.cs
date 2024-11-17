namespace Guppy.Core.Common.Contexts
{
    public class ServiceLoggerContext
    {
        public Type ServiceType { get; init; } = typeof(object);
        public string LoggerContext { get; init; } = string.Empty;
    }
}
