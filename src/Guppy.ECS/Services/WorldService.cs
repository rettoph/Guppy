using Guppy.Common;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Guppy.ECS.Services
{
    internal sealed class WorldService : IWorldService
    {
        private IEnumerable<ISystem> _systems;

        public World Instance { get; }

        public WorldService(IFiltered<ISystem> systems)
        {
            _systems = systems.Items;

            WorldBuilder builder = new WorldBuilder();

            foreach (ISystem system in _systems)
            {
                builder.AddSystem(system);
            }

            this.Instance = builder.Build();
        }
    }
}
