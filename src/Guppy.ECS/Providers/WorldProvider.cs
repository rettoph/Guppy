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

namespace Guppy.ECS.Providers
{
    internal sealed class WorldProvider : IWorldProvider
    {
        private ISystemDefinition[] _definitions;

        public WorldProvider(IEnumerable<ISystemDefinition> definitions)
        {
            _definitions = definitions.OrderBy(d => d.Order).ToArray();
        }

        public World Create(IServiceProvider provider)
        {
            WorldBuilder builder = new WorldBuilder();

            foreach(ISystemDefinition definition in _definitions)
            {
                if (definition.Filter(provider))
                {
                    ISystem system = (ISystem)ActivatorUtilities.CreateInstance(provider, definition.Type);
                    builder.AddSystem(system);
                }
            }

            World world = builder.Build();

            return world;
        }
    }
}
