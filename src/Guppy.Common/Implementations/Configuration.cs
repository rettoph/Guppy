﻿using Autofac;
using Autofac.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal class Configuration<T> : IConfiguration<T>
        where T : new()
    {
        public T Value { get; }

        public Configuration(ILifetimeScope scope, IEnumerable<ConfigurationBuilder<T>> builders)
        {
            this.Value = new();

            foreach(var builder in builders)
            {
                builder.Build(scope, this.Value);
            }
        }
    }

    internal class ConfigurationBuilder<T>
        where T : new()
    {
        private Action<ILifetimeScope, T> _builder;

        public ConfigurationBuilder(Action<ILifetimeScope, T> builder)
        {
            _builder = builder;
        }

        public void Build(ILifetimeScope scope, T instance)
        {
            _builder(scope, instance);
        }
    }
}
