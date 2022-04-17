using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public abstract class Component<TEntity> : IComponent
        where TEntity : IEntity
    {
        public readonly TEntity Entity;

        public virtual void Dispose()
        {

        }

        public virtual void Initialize()
        {

        }
    }
}
