using Guppy.Attributes;
using Guppy.Gaming.Definitions;
using Guppy.Gaming.Services;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Initializers
{
    internal sealed class InputInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var inputs = assemblies.GetAttributes<InputDefinition, AutoLoadAttribute>().Types;

            foreach (Type input in inputs)
            {
                services.AddInput(input);
            }

            services.AddSingleton<IInputService, InputService>();
        }
    }
}
