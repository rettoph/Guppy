using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Resources.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class PolymorphicJsonTypeAttribute : InitializableAttribute
    {
        public readonly string Key;

        public PolymorphicJsonTypeAttribute(string key)
        {
            this.Key = key;
        }

        public override void Initialize(IServiceCollection services, Type classType)
        {
            base.Initialize(services, classType);

            services.AddSingleton<PolymorphicJsonType>(new PolymorphicJsonType(this.Key, classType));
        }
    }
}
