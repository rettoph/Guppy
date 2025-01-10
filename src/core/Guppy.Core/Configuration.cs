using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core
{
    public class Configuration<T> : IConfiguration<T>
        where T : new()
    {
        public T Value { get; }

        public Configuration(IConfigurationService configurations)
        {
            this.Value = new T();
            configurations.Configure(this.Value);
        }
    }
}