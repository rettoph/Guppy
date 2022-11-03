using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoSubscribeAttribute : InitializableAttribute
    {
        public AutoSubscribeAttribute()
        {
        }

        public override void Initialize(IServiceCollection services, Type classType)
        {
            base.Initialize(services, classType);

            services.AddAlias(new Alias(typeof(ISubscriber), classType));
        }
    }
}
