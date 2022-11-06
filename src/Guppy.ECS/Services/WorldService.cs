using Guppy.Common;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;

namespace Guppy.ECS.Services
{
    internal sealed class WorldService : IWorldService
    {
        public World Instance { get; }

        public WorldService(IEnumerable<ComponentType> componentTypes, IFiltered<ISystem> systems)
        {
            this.Instance = new World(componentTypes, systems.Items);
        }
    }
}
