using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Configurations;
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
    public sealed class PolymorphicJsonTypeAttribute : GuppyConfigurationAttribute
    {
        public readonly string Key;

        public PolymorphicJsonTypeAttribute(string key)
        {
            this.Key = key;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            configuration.Services.AddSingleton<PolymorphicJsonType>(new PolymorphicJsonType(this.Key, classType));
        }
    }
}
