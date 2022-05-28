using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.EntityComponent.Services;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    internal sealed class ComponentProvider : IComponentProvider
    {
        private IServiceProvider _provider;
        private Dictionary<Type, EntityComponentFilters[]> _ecfs;

        public ComponentProvider(
            IServiceProvider provider,
            ITypeProvider<IEntity> entities,
            IEnumerable<ComponentDefinition> componentDefinitions,
            IEnumerable<ComponentFilterDefinition> filterDefinitions)
        {
            var filters = filterDefinitions.Select(x => x.BuildComponentFilter());
            var ecfs = new List<EntityComponentFilters>();

            _provider = provider;
            _ecfs = new Dictionary<Type, EntityComponentFilters[]>(entities.Count());

            foreach (Type entity in entities)
            {
                var entityComponents = componentDefinitions.Where(c => c.EntityType.IsAssignableFrom(entity));

                foreach (ComponentDefinition component in entityComponents)
                {
                    var comonentFilters = filters.Where(f => f.ComponentType.IsAssignableFrom(component.ComponentType) && f.TypeFilter(entity, component));
                    var ecf = new EntityComponentFilters(entity, component, comonentFilters.ToArray());
                    ecfs.Add(ecf);
                }

                _ecfs.Add(entity, ecfs.ToArray());
                ecfs.Clear();
            }
        }

        public IComponentService Create(IEntity entity)
        {
            EntityComponentFilters[] ecfs = _ecfs[entity.GetType()];
            IComponentService components = new ComponentService(entity, ecfs.Length);
            foreach(EntityComponentFilters ecf in ecfs)
            {
                if(ecf.Filter(_provider, entity))
                {
                    IComponent component = ecf.ComponentDefinition.BuildComponent(_provider);
                    components.Add(ecf.ComponentDefinition.ComponentType, component);
                }
            }

            return components;
        }
    }
}
