using System.Reflection;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Extensions.System.Collections.Generic;
using Guppy.Core.Common.Utilities;

namespace Guppy.Core.Common
{
    public sealed class GuppyEnvironmentBuilder
    {
        private readonly HashSet<Assembly> _libraries;

        public string Company { get; }

        public string Project { get; }

        public Assembly Entry { get; }

        public IEnumerable<Assembly> Libraries => this._libraries;

        public List<IEnvironmentVariable> CustomVariables { get; }

        public GuppyEnvironmentBuilder(
            string company,
            string name,
            IEnumerable<Assembly>? libraries = null,
            Assembly? entry = null)
        {
            this._libraries = [];

            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();

            this.Company = company;
            this.Project = name;
            this.Entry = entry;
            this.CustomVariables = [];

            this._libraries.AddRange(libraries ?? Enumerable.Empty<Assembly>());
        }

        public GuppyEnvironmentBuilder AddLibrary(Assembly assembly)
        {
            this._libraries.Add(assembly);

            return this;
        }

        public GuppyEnvironmentBuilder Add(IEnvironmentVariable variable)
        {
            this.CustomVariables.Add(variable);

            return this;
        }

        public GuppyEnvironment Build()
        {
            // Build initial environment variables dictionary with required values.
            Dictionary<Type, IEnvironmentVariable> environmentVariables = new()
            {
                [typeof(GuppyEnvironmentVariables.Project)] = GuppyEnvironmentVariables.Project.Create(this.Project),
                [typeof(GuppyEnvironmentVariables.Company)] = GuppyEnvironmentVariables.Company.Create(this.Company),
                [typeof(GuppyEnvironmentVariables.LibraryAssemblies)] = GuppyEnvironmentVariables.LibraryAssemblies.Create([.. this.Libraries]),
                [typeof(GuppyEnvironmentVariables.EntryAssembly)] = GuppyEnvironmentVariables.EntryAssembly.Create(this.Entry ?? Assembly.GetEntryAssembly() ?? throw new NotImplementedException())
            };

            // Populate global environment variables with custom values
            foreach (IEnvironmentVariable customVariable in this.CustomVariables)
            {
                environmentVariables[customVariable.GetType()] = customVariable;
            }

            return new GuppyEnvironment(environmentVariables);
        }
    }
}