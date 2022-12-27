using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Filters
{
    public class ConfigurationFilter : SimpleFilter
    {
        private readonly object? _configuration;

        public ConfigurationFilter(Type type, object? configuration) : base(type)
        {
            _configuration = configuration;
        }

        public override bool Invoke(IServiceProvider provider, IServiceConfiguration service, object? configuration)
        {
            if (_configuration is null && configuration is null)
            {
                return true;
            }

            if (_configuration is null)
            {
                return false;
            }

            if(configuration is null)
            {
                return false;
            }

            var result = _configuration.Equals(configuration);

            return result;
        }
    }

    public class ConfigurationFilter<T> : ConfigurationFilter
    {
        public ConfigurationFilter(object? configuration) : base(typeof(T), configuration)
        {
        }
    }
}
