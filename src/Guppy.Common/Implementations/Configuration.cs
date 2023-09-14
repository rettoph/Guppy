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

        public Configuration(IEnumerable<ConfigurationBuilder<T>> builders)
        {
            this.Value = new();

            foreach(var builder in builders)
            {
                builder.Build(this.Value);
            }
        }
    }

    internal class ConfigurationBuilder<T>
        where T : new()
    {
        private Action<T> _builder;

        public ConfigurationBuilder(Action<T> builder)
        {
            _builder = builder;
        }

        public void Build(T instance)
        {
            _builder(instance);
        }
    }
}
