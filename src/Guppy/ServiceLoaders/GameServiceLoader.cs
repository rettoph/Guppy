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
        public void RegisterServices(DependencyInjection.GuppyServiceCollection services)
        {
            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute<Game>(false).ForEach(g =>
            {
                services.RegisterGame(game: g);
            });
        }

        public void ConfigureProvider(DependencyInjection.GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
