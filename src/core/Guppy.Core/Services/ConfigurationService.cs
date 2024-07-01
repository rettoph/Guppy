﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class ConfigurationService : IConfigurationService
    {
        private readonly ILifetimeScope _scope;
        private readonly ConfigurationBuilder[] _builders;

        public ConfigurationService(ILifetimeScope scope)
        {
            _scope = scope;
            _builders = _scope.Resolve<IEnumerable<ConfigurationBuilder>>().ToArray();
        }

        public void Configure<T>(T instance)
        {
            if (instance is null)
            {
                return;
            }

            foreach (ConfigurationBuilder builder in _builders)
            {
                if (builder.CanBuild(instance.GetType()))
                {
                    builder.Build(_scope, instance);
                }
            }
        }
    }
}
