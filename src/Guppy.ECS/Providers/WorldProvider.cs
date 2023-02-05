using Guppy.Common;
using Guppy.Common.Providers;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;

namespace Guppy.ECS.Providers
{
    internal sealed class WorldProvider : IWorldProvider
    {
        private readonly IFilteredProvider _filteredProvider;

        public WorldProvider(IFilteredProvider filteredProvider)
        {
            _filteredProvider = filteredProvider;
        }

        public World Get(object? configuration = null)
        {
            var systems = _filteredProvider.Instances<ISystem>().Sort();
            var builder = new WorldBuilder();

            foreach (var system in systems)
            {
                builder.AddSystem(system);
            }

            return builder.Build();
        }
    }
}
