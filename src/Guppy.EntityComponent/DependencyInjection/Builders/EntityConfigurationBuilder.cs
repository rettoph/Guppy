using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public class EntityConfigurationBuilder<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Fields
        private ServiceProviderBuilder _services;
        #endregion

        #region Constructors
        public EntityConfigurationBuilder(ServiceProviderBuilder services)
        {
            _services = services;
        }
        #endregion

        #region RegisterEntityServiceConfiguration Methods
        public EntityConfigurationBuilder<TEntity> RegisterService<T>(String name, Action<ServiceConfigurationBuilder<T>> builder)
            where T : class, TEntity
        {
            ServiceConfigurationBuilder<T> service = _services.RegisterService<T>(name);
            builder(service);

            return this;
        }
        public EntityConfigurationBuilder<TEntity> RegisterService<T>(Action<ServiceConfigurationBuilder<T>> builder)
            where T : class, TEntity
        {
            return this.RegisterService<T>(typeof(T).FullName, builder);
        }
        public EntityConfigurationBuilder<TEntity> RegisterService(String name, Action<ServiceConfigurationBuilder<TEntity>> builder)
        {
            return this.RegisterService<TEntity>(name, builder);
        }
        public EntityConfigurationBuilder<TEntity> RegisterService(Action<ServiceConfigurationBuilder<TEntity>> builder)
        {
            return this.RegisterService<TEntity>(builder);
        }
        #endregion

        #region RegisterComponentConfiguration Methods
        public EntityConfigurationBuilder<TEntity> RegisterComponent<TComponent>(String name, Action<ComponentConfigurationBuilder<TComponent>> builder)
            where TComponent : Component<TEntity>
        {
            ComponentConfigurationBuilder<TComponent> componentConfiguration = _services.RegisterComponent<TComponent>(name);
            componentConfiguration.SetAssignableEntityType<TEntity>();
            builder(componentConfiguration);

            return this;
        }
        public EntityConfigurationBuilder<TEntity> RegisterComponent<TComponent>(Action<ComponentConfigurationBuilder<TComponent>> builder)
            where TComponent : Component<TEntity>
        {
            return this.RegisterComponent<TComponent>(typeof(TComponent).FullName, builder);
        }
        #endregion
    }
}
