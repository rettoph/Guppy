using Guppy.Common.Contexts;
using System.Reflection;

namespace Guppy
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
            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat([
                typeof(GuppyEngine).Assembly
            ]).ToArray();

            this.Company = company;
            this.Name = name;
            this.Entry = entry;
            this.Libraries = libraries;
        }
    }
}
