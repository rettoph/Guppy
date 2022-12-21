using Guppy.Common;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;

namespace Guppy.ECS.Services
{
    internal sealed class WorldService : IWorldService
    {
        public World Instance { get; }

        public WorldService(IFiltered<ISystem> systems)
        {
            var builder = new WorldBuilder();

            foreach(var system in systems.Items)
            {
                builder.AddSystem(system);
            }

            this.Instance = builder.Build();
        }
    }
}
