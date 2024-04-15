using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Contexts;
using Guppy.Engine.Common;
using System.Reflection;

namespace Guppy.Engine
{
    public sealed class GuppyContext : IGuppyContext
    {
        public string Company { get; }

        public string Name { get; }

        public Assembly Entry { get; }

        public IEnumerable<Assembly> Libraries { get; }

        public GuppyContext(string company, string name, IEnumerable<Assembly>? libraries = null, Assembly? entry = null)
        {
            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();

            if (libraries is null)
            {
                libraries = Enumerable.Empty<Assembly>();
            }
            else
            {
                libraries = libraries.Concat([
                    typeof(GuppyConfigurationAttribute).Assembly,
                    typeof(GuppyEngine).Assembly,
                    typeof(IGuppyEngine).Assembly
                ]).ToArray();
            }

            this.Company = company;
            this.Name = name;
            this.Entry = entry;
            this.Libraries = libraries;
        }
    }
}
