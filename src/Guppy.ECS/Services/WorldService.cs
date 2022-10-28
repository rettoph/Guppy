using Guppy.ECS.Definitions;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Services
{
    internal sealed class WorldService : IWorldService, IDisposable
    {
        private IEnumerable<ISystem> _systems;

        public World Instance { get; }

        public WorldService(IEnumerable<ISystem> systems)
        {
            _systems = systems;

            WorldBuilder builder = new WorldBuilder();

            foreach (ISystem system in systems)
            {
                builder.AddSystem(system);
            }

            this.Instance = builder.Build();
        }

        public void Dispose()
        {
            foreach(ISystem system in _systems)
            {
                system.Dispose();
            }
        }
    }
}
