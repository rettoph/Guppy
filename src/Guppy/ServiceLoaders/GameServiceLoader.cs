using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GameServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            assemblyHelper.Types.GetTypesWithAutoLoadAttribute<Game>(false).ForEach(gameType =>
            {
                services.RegisterGame(game: gameType)
                    .SetTypeFactory(factory =>
                    {
                        factory.SetMethod(p => Activator.CreateInstance(gameType) as Game);
                    });
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
