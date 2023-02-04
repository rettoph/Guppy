using Guppy.Attributes.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    public class ScopedAttribute : InitializableAttribute
    {
        public readonly Type? Type;

        public ScopedAttribute(Type? type = null)
        {
            this.Type = type;
        }

        public override void Initialize(IServiceCollection services, Type classType)
        {
            base.Initialize(services, classType);

            if(!classType.HasCustomAttribute<AutoLoadAttribute>())
            {
                return;
            }

            services.AddScoped(this.Type ?? classType, classType);
        }
    }

    public class ScopedAttribute<TService> : SingletonAttribute
    {
        public ScopedAttribute() : base(typeof(TService))
        {

        }
    }
}
