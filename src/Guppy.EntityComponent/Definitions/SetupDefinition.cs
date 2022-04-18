using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions
{
    public abstract class SetupDefinition
    {
        public virtual Type EntityType { get; } = typeof(IEntity);
        public virtual int Order { get; } = 0;

        protected abstract bool TryCreate(IServiceProvider provider, IEntity entity);
        protected abstract bool TryDestroy(IServiceProvider provider, IEntity entity);

        public virtual Setup BuildSetup()
        {
            return new Setup(this.EntityType, this.TryCreate, this.TryDestroy, this.Order);
        }
    }

    public abstract class SetupDefinition<TEntity> : SetupDefinition
        where TEntity : class, IEntity
    {
        public override Type EntityType { get; } = typeof(TEntity);

        protected override bool TryCreate(IServiceProvider provider, IEntity entity)
        {
            return entity is TEntity casted && this.TryCreate(provider, casted);
        }

        protected override bool TryDestroy(IServiceProvider provider, IEntity entity)
        {
            return entity is TEntity casted && this.TryDestroy(provider, casted);
        }

        protected abstract bool TryCreate(IServiceProvider provider, TEntity entity);
        protected abstract bool TryDestroy(IServiceProvider provider, TEntity entity);
    }
}
