using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GameServiceLoader : IServiceLoader
    {
        public void RegisterServices(DependencyInjection.ServiceCollection services)
        {
            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<Game>(false).ForEach(g =>
            {
                services.AddGame(g, p => ActivatorUtilities.CreateInstance(p, g));
            });
        }

        public void ConfigureProvider(DependencyInjection.ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
