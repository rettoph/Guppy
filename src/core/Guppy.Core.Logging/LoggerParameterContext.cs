namespace Guppy.Core.Logging
{
    internal readonly struct LoggerParameterContext(Type contextType, Type parameterType)
    {
        public readonly Type ContextType = contextType;
        public readonly Type ParameterType = parameterType;
    }
}