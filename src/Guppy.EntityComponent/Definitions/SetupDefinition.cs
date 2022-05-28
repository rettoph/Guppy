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

        protected virtual void Load(IServiceProvider provider)
        {
            // 
        }

        protected abstract bool TryInitialize(IEntity entity);
        protected abstract bool TryUninitialize(IEntity entity);

        public virtual Setup BuildSetup()
        {
            return new Setup(this.EntityType, this.Load, this.TryInitialize, this.TryUninitialize, this.Order);
        }
    }

    public abstract class SetupDefinition<TEntity> : SetupDefinition
        where TEntity : class, IEntity
    {
        public override Type EntityType { get; } = typeof(TEntity);

        protected override bool TryInitialize(IEntity entity)
        {
            return entity is TEntity casted && this.TryInitialize(casted);
        }

        protected override bool TryUninitialize(IEntity entity)
        {
            return entity is TEntity casted && this.TryUninitialize(casted);
        }

        protected abstract bool TryInitialize(TEntity entity);
        protected abstract bool TryUninitialize(TEntity entity);
    }
}
