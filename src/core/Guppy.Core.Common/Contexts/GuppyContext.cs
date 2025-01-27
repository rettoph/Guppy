using System.Reflection;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Extensions.System.Collections.Generic;

namespace Guppy.Core.Common
{
    public sealed class GuppyContext : IGuppyContext
    {
        private readonly HashSet<Assembly> _libraries;

        public string Company { get; }

        public string Name { get; }

        public Assembly Entry { get; }

        public IEnumerable<Assembly> Libraries => this._libraries;

        public GuppyContext(string company, string name, IEnumerable<Assembly>? libraries = null, Assembly? entry = null)
        {
            this._libraries = [];

            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();

            this.Company = company;
            this.Name = name;
            this.Entry = entry;

            this._libraries.AddRange(libraries ?? Enumerable.Empty<Assembly>());
        }

        public IGuppyContext AddLibrary(Assembly assembly)
        {
            this._libraries.Add(assembly);

            return this;
        }
    }
}