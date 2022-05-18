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
        public Action<IServiceProvider> Initialize;
        public Func<IEntity, bool> TryCreate;
        public Func<IEntity, bool> TryDestroy;
        public readonly int Order;

        public Setup(Type entityType, Action<IServiceProvider> initialize, Func<IEntity, bool> tryCreate, Func<IEntity, bool> tryDestroy, int order)
        {
            this.EntityType = entityType;
            this.Initialize = initialize;
            this.TryCreate = tryCreate;
            this.TryDestroy = tryDestroy;
            this.Order = order;
        }
    }
}
