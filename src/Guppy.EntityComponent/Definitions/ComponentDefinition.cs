using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions
{
    public abstract class ComponentDefinition
    {
        private static readonly object[] EmptyArgs = Array.Empty<object>();

        public abstract Type EntityType { get; }
        public abstract Type ComponentType { get; }
        public abstract Func<IServiceProvider, IEntity, IComponent> BuildComponent { get; }

        public static Func<IServiceProvider, IEntity, TComponent> DynamicFactory<TEntity, TComponent>()
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            try
            {
                ObjectFactory factoryWithEntity = ActivatorUtilities.CreateFactory(typeof(TComponent), new[] { typeof(TEntity) });
                object[] argBuffer = new object[1];
                TComponent FactoryWithEntity(IServiceProvider provider, IEntity entity)
                {
                    argBuffer[0] = entity;
                    return factoryWithEntity(provider, argBuffer) as TComponent ?? throw new Exception();
                }

                return FactoryWithEntity;
            }
            catch
            {
                ObjectFactory factoryWithoutEntity = ActivatorUtilities.CreateFactory(typeof(TComponent), Array.Empty<Type>());
                TComponent FactoryWithoutEntity(IServiceProvider provider, IEntity entity)
                {
                    return factoryWithoutEntity(provider, EmptyArgs) as TComponent ?? throw new Exception();
                }

                return FactoryWithoutEntity;
            }
        }
    }

    public abstract class ComponentDefinition<TEntity, TComponent> : ComponentDefinition
        where TEntity : class, IEntity
        where TComponent : class, IComponent
    {
        private Func<IServiceProvider, IEntity, TComponent> _factory = ComponentDefinition.DynamicFactory<TEntity, TComponent>();

        public override Type EntityType { get; } = typeof(TEntity);
        public override Type ComponentType { get; } = typeof(TComponent);
        public override Func<IServiceProvider, IEntity, IComponent> BuildComponent { get; }

        public ComponentDefinition()
        {
            this.BuildComponent = this.Factory;
        }

        public virtual TComponent Factory(IServiceProvider provider, IEntity entity)
        {
            return _factory(provider, entity);
        }
    }
}
