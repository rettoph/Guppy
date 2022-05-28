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
        public Action<IServiceProvider> Load;
        public Func<IEntity, bool> TryInitialize;
        public Func<IEntity, bool> TryUninitialize;
        public readonly int Order;

        public Setup(Type entityType, Action<IServiceProvider> load, Func<IEntity, bool> tryInitialize, Func<IEntity, bool> tryUninitialize, int order)
        {
            this.EntityType = entityType;
            this.Load = load;
            this.TryInitialize = tryInitialize;
            this.TryUninitialize = tryUninitialize;
            this.Order = order;
        }
    }
}
