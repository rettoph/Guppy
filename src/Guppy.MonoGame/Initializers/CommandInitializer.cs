using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Initializers
{
    internal sealed class CommandInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var definitions = assemblies.GetTypes<ICommandDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach (Type definition in definitions)
            {
                services.AddCommand(definition);
            }

            services.AddSingleton<ICommandService, CommandService>();
        }
    }
}
