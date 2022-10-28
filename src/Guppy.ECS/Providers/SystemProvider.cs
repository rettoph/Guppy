using Guppy.ECS.Definitions;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Providers
{
    public class SystemProvider : ISystemProvider
    {
        private ISystemDefinition[] _definitions;

        public SystemProvider(IEnumerable<ISystemDefinition> definitions)
        {
            _definitions = definitions.OrderBy(d => d.Order).ToArray();
        }

        public IEnumerable<ISystem> Create(IServiceProvider provider)
        {
            var systems = new List<ISystem>();

            foreach (ISystemDefinition definition in _definitions)
            {
                if (definition.Filter(provider))
                {
                    ISystem system = (ISystem)ActivatorUtilities.CreateInstance(provider, definition.Type);
                    systems.Add(system);
                }
            }

            return systems;
        }
    }
}
