namespace Guppy.Core.Common.Contexts
{
    public class LoggerContext
    {
        public Type ServiceType { get; init; } = typeof(object);
        public string Context { get; init; } = string.Empty;
    }
}
