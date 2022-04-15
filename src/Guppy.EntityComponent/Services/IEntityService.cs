using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    public interface IEntityService : IEnumerable<IEntity>
    {
        event OnEventDelegate<IEntityService, IEntity>? OnEntityAdded;
        event OnEventDelegate<IEntityService, IEntity>? OnEntityRemoved;

        bool TryAdd(IEntity entity);

        TEntity Create<TEntity>()
            where TEntity : IEntity;

        IEntity Create(Type type);

        bool TryRemove(IEntity entity);
    }
}
