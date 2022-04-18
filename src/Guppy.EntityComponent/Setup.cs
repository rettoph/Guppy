using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public sealed class Setup
    {
        public readonly Type EntityType;
        public Func<IServiceProvider, IEntity, bool> TryCreate;
        public Func<IServiceProvider, IEntity, bool> TryDestroy;
        public readonly int Order;

        public Setup(Type entityType, Func<IServiceProvider, IEntity, bool> tryCreate, Func<IServiceProvider, IEntity, bool> tryDestroy, int order)
        {
            this.EntityType = entityType;
            this.TryCreate = tryCreate;
            this.TryDestroy = tryDestroy;
            this.Order = order;
        }
    }
}
