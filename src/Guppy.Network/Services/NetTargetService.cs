using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Services.Common;
using Minnow.Collections;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    internal sealed class NetTargetService : CollectionService<ushort, INetTarget>, INetTargetService
    {
        private readonly IEntityService _entities;

        private readonly Map<Type, uint> _map;

        public Map<Type, uint> Map => _map;

        public NetTargetService(IEntityService entities, ITypeProvider<INetTarget> types)
        {
            _entities = entities;
            _map = types.ToMap(x => x.xxHash());
        }

        public bool TryCreate(uint hash, [MaybeNullWhen(false)] out INetTarget target)
        {
            if(_entities.TryCreate(_map[hash], out IEntity? entity) && entity is INetTarget casted)
            {
                target = casted;
                return true;
            }

            target = null;
            return false;
        }

        protected override ushort GetKey(INetTarget item)
        {
            return item.NetId;
        }

        bool INetTargetService.TryAdd(INetTarget target)
        {
            return this.items.TryAdd(this.GetKey(target), target);
        }

        bool INetTargetService.TryRemove(INetTarget target)
        {
            return this.items.Remove(this.GetKey(target));
        }
    }
}
