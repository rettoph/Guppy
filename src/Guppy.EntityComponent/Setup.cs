using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public abstract class Setup : ISetup
    {
        public virtual Type EntityType => typeof(IEntity);

        public virtual int Order => 0;

        public abstract bool TryCreate(IEntity entity);
        public abstract bool TryDestroy(IEntity entity);
    }

    public abstract class Setup<TEntity> : Setup
        where TEntity : IEntity
    {
        public override Type EntityType => typeof(TEntity);

        public override bool TryCreate(IEntity entity)
        {
            if(entity is TEntity casted)
            {
                return this.TryCreate(casted);
            }

            return false;
        }
        public override bool TryDestroy(IEntity entity)
        {
            if (entity is TEntity casted)
            {
                return this.TryDestroy(casted);
            }

            return false;
        }

        protected abstract bool TryCreate(IServiceProvider provider, TEntity entity);
        protected abstract bool TryDestroy(IServiceProvider provider, TEntity entity);
    }
}
