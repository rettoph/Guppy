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
        public abstract Func<IServiceProvider, IComponent> BuildComponent { get; }

        public static Func<IServiceProvider, TComponent> DynamicFactory<TComponent>()
            where TComponent : class, IComponent
        {
            ObjectFactory factoryWithoutEntity = ActivatorUtilities.CreateFactory(typeof(TComponent), Array.Empty<Type>());
            TComponent FactoryMethod(IServiceProvider provider)
            {
                return factoryWithoutEntity(provider, EmptyArgs) as TComponent ?? throw new Exception();
            }

            return FactoryMethod;
        }
    }

    public abstract class ComponentDefinition<TEntity, TComponent> : ComponentDefinition
        where TEntity : class, IEntity
        where TComponent : class, IComponent
    {
        private Func<IServiceProvider, TComponent> _factory = ComponentDefinition.DynamicFactory<TComponent>();

        public override Type EntityType { get; } = typeof(TEntity);
        public override Type ComponentType { get; } = typeof(TComponent);
        public override Func<IServiceProvider, IComponent> BuildComponent { get; }

        public ComponentDefinition()
        {
            this.BuildComponent = this.Factory;
        }

        public virtual TComponent Factory(IServiceProvider provider)
        {
            return _factory(provider);
        }
    }
}
