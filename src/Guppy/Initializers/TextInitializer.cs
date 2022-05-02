using Guppy.Attributes;
using Guppy.Definitions;
using Guppy.Loaders;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    [AutoLoad]
    internal sealed class TextInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var texts = assemblies.GetAttributes<TextDefinition, AutoLoadAttribute>().Types;

            foreach (Type text in texts)
            {
                services.AddText(text);
            }

            services.AddSingleton<ILanguageProvider, LanguageProvider>();
        }
    }
}
