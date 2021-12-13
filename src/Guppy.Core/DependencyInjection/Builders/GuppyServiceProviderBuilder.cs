using DotNetUtils.DependencyInjection;
using DotNetUtils.DependencyInjection.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public class GuppyServiceProviderBuilder : ServiceProviderBuilder<GuppyServiceProvider>
    {
        #region Private Fields
        private List<ComponentConfigurationBuilder> _componentConfigurations;
        private List<ComponentFilterBuilder> _componentFilters;
        #endregion

        #region Constructors
        public GuppyServiceProviderBuilder()
        {
            _componentConfigurations = new List<ComponentConfigurationBuilder>();
            _componentFilters = new List<ComponentFilterBuilder>();
        }
        #endregion

        #region RegisterComponentConfiguration Methods
        public ComponentConfigurationBuilder RegisterComponent(String componentName)
        {
            ComponentConfigurationBuilder componentConfiguration = new ComponentConfigurationBuilder(componentName);
            _componentConfigurations.Add(componentConfiguration);

            return componentConfiguration;
        }

        public ComponentConfigurationBuilder RegisterComponent<TComponent>()
        {
            return this.RegisterComponent(typeof(TComponent).FullName);
        }
        #endregion

        #region RegisterComponentTypeFilter Methods
        public ComponentFilterBuilder RegisterComponentFilter(Type assignableComponentType)
        {
            ComponentFilterBuilder componentFilter = new ComponentFilterBuilder(assignableComponentType);
            _componentFilters.Add(componentFilter);

            return componentFilter;
        }

        public ComponentFilterBuilder RegisterComponentFilter<TAssignableComponentType>()
        {
            ComponentFilterBuilder componentFilter = new ComponentFilterBuilder(typeof(TAssignableComponentType));
            _componentFilters.Add(componentFilter);

            return componentFilter;
        }
        #endregion

        #region ServiceProviderBuilder<GuppyServiceProvider> Implementation
        protected override GuppyServiceProvider Build(Dictionary<String, ServiceConfiguration<GuppyServiceProvider>> services)
        {
            List<ComponentFilter> componentFilters = _componentFilters.Order().Select(cf => cf.Build()).ToList();
            List<ComponentConfiguration> componentConfigurations = _componentConfigurations.Order().Select(c => c.Build(componentFilters, services)).ToList();
            List<(ServiceConfiguration<GuppyServiceProvider> entity, ComponentConfiguration[] components)> entityComponentConfigurations = new List<(ServiceConfiguration<GuppyServiceProvider> entity, ComponentConfiguration[] components)>();

            foreach(ServiceConfiguration<GuppyServiceProvider> service in services.Values)
            {
                if(typeof(IEntity).IsAssignableFrom(service.TypeFactory.Type))
                {
                    entityComponentConfigurations.Add((
                        entity: service,
                        components: componentConfigurations.Where(cc => cc.EntityServiceConfigurations
                            .Any(ec => ec == service))
                            .ToArray() ?? Array.Empty<ComponentConfiguration>()
                    ));
                }
            }

            // Create lookup dictionary for entity configuration => 
            Dictionary<UInt32, ComponentConfiguration[]> entityComponentConfigurationsTable = entityComponentConfigurations.ToDictionary(
                keySelector: tuple => tuple.entity.Id,
                elementSelector: tuple => tuple.components);

            return new GuppyServiceProvider(
                services: services,
                entityComponentConfigurations: entityComponentConfigurationsTable);
        }
        #endregion
    }
}
