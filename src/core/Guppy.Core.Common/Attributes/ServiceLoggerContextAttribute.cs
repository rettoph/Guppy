using Autofac;
using Guppy.Core.Common.Extensions.Autofac;

namespace Guppy.Core.Common.Attributes
{
    public class ServiceLoggerContextAttribute : GuppyConfigurationAttribute
    {
        public readonly string LoggerContext;
        public readonly Type? ServiceType;

        internal ServiceLoggerContextAttribute(string name, Type? serviceType)
        {
            this.LoggerContext = name;
            this.ServiceType = serviceType;
        }

        public ServiceLoggerContextAttribute(string name) : this(name, null)
        {

        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            Type type = this.ServiceType ?? classType;

            ThrowIf.Type.IsNotAssignableFrom(type, classType);

            builder.RegisterServiceLoggerContext(type, this.LoggerContext);
        }
    }

    public class LoggerContextAttribute<TServiceType> : ServiceLoggerContextAttribute
    {
        public LoggerContextAttribute(string name) : base(name, typeof(TServiceType))
        {

        }
    }
}
