using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Core.Logging.Common.Configurations
{
    public class LoggerConfiguration
    {
        private readonly Dictionary<string, object> _enrichments = [];
        private readonly Dictionary<Type, LogMessageParameterTypeEnum> _parameterTypes = [];

        public LoggerConfiguration EnrichWith(string name, object value)
        {
            this._enrichments[name] = value;

            return this;
        }

        public LoggerConfiguration SetParameterType(LogMessageParameterTypeEnum parameterType, params Type[] types)
        {
            foreach (Type type in types)
            {
                this._parameterTypes[type] = parameterType;
            }

            return this;
        }

        public LoggerConfiguration SetParameterTypeScalar<T>()
        {
            return this.SetParameterType(LogMessageParameterTypeEnum.Scalar, typeof(T));
        }

        public IEnumerable<KeyValuePair<string, object>> GetEnrichments()
        {
            return this._enrichments;
        }

        public IEnumerable<KeyValuePair<Type, LogMessageParameterTypeEnum>> GetParameterTypes()
        {
            return this._parameterTypes;
        }
    }
}
