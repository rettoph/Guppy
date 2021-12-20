using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection.Builders;
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
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            assemblyHelper.Types.GetTypesWithAutoLoadAttribute<Game>(false).ForEach(gameType =>
            {
                services.RegisterGame(game: gameType)
                    .RegisterTypeFactory(factory =>
                    {
                        factory.SetMethod(p => Activator.CreateInstance(gameType) as Game);
                    });
            });
        }
    }
}
