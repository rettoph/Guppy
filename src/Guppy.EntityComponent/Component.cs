using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public abstract class Component<TEntity> : IComponent
        where TEntity : class, IEntity
    {
        void IComponent.Initialize(IEntity entity)
        {
            this.Initialize(entity as TEntity ?? throw new InvalidCastException());
        }

        protected abstract void Initialize(TEntity entity);

        void IComponent.Uninitilaize()
        {
            this.Uninitilaize();
        }

        protected abstract void Uninitilaize();

        public virtual void Dispose()
        {

        }
    }
}
