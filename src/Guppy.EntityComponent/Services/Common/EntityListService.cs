using Guppy.EntityComponent;
using Guppy.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services.Common
{
    public abstract class EntityListService<T> : ListService<Guid, T>
        where T : class, IEntity
    {
        private IEntityService _entities;

        protected EntityListService(
            IEntityService entities, 
            IServiceProvider provider) : base(provider)
        {
            _entities = entities;
        }

        protected override Guid GetId(T item)
        {
            return item.Id;
        }

        protected override TItem? Create<TItem>(IServiceProvider provider)
            where TItem : class
        {
            TItem? item = base.Create<TItem>(provider);

            if(item is not null && _entities.TryAdd(item))
            {
                return item;
            }

            return default;
        }
    }
}
