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
    public class SingletonAttribute : InitializableAttribute
    {
        public readonly Type? Type;

        public SingletonAttribute(Type? type = null)
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

            services.AddSingleton(this.Type ?? classType, classType);
        }
    }

    public class SingletonAttribute<TService> : SingletonAttribute
    {
        public SingletonAttribute() : base(typeof(TService))
        {

        }
    }
}
