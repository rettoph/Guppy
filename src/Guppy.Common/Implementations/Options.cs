using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal class Options<T> : IOptions<T>
        where T : new()
    {
        public T Value { get; }

        public Options(IEnumerable<OptionBuilder<T>> builders)
        {
            this.Value = new();

            foreach(var builder in builders)
            {
                builder.Build(this.Value);
            }
        }
    }

    internal class OptionBuilder<T>
        where T : new()
    {
        private Action<T> _builder;

        public OptionBuilder(Action<T> builder)
        {
            _builder = builder;
        }

        public void Build(T instance)
        {
            _builder(instance);
        }
    }
}
