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

        protected virtual void Initialize(IServiceProvider provider)
        {
            // 
        }

        protected abstract bool TryCreate(IEntity entity);
        protected abstract bool TryDestroy(IEntity entity);

        public virtual Setup BuildSetup()
        {
            return new Setup(this.EntityType, this.Initialize, this.TryCreate, this.TryDestroy, this.Order);
        }
    }

    public abstract class SetupDefinition<TEntity> : SetupDefinition
        where TEntity : class, IEntity
    {
        public override Type EntityType { get; } = typeof(TEntity);

        protected override bool TryCreate(IEntity entity)
        {
            return entity is TEntity casted && this.TryCreate(casted);
        }

        protected override bool TryDestroy(IEntity entity)
        {
            return entity is TEntity casted && this.TryDestroy(casted);
        }

        protected abstract bool TryCreate(TEntity entity);
        protected abstract bool TryDestroy(TEntity entity);
    }
}
