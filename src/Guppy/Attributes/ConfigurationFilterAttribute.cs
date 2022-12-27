using Guppy.Common.Attributes;
using Guppy.Common;
using Guppy.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    public sealed class ConfigurationFilterAttribute : InitializableAttribute
    {
        public readonly object? Configuration;

        public ConfigurationFilterAttribute(object? configuration)
        {
            this.Configuration = configuration;
        }

        public override void Initialize(IServiceCollection services, Type classType)
        {
            services.AddFilter(new ConfigurationFilter(classType, this.Configuration));
        }
    }
}
