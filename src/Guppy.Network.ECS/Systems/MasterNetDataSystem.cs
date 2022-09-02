using Guppy.Network.ECS.Components;
using Guppy.Network.ECS.Services;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Systems
{
    internal sealed class MasterNetDataSystem : EntitySystem
    {
        private NetDataIdProvider _ids;
        private ComponentMapper<NetData> _netDataMapper;
        public MasterNetDataSystem(NetDataIdProvider ids) : base(Aspect.All(typeof(NetData)))
        {
            _ids = ids;
            _netDataMapper = null!;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _netDataMapper = mapperService.GetMapper<NetData>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            base.OnEntityAdded(entityId);

            _ids.Reserve(out _netDataMapper.Get(entityId).Id);
        }

        protected override void OnEntityRemoved(int entityId)
        {
            base.OnEntityRemoved(entityId);

            _ids.Release(in _netDataMapper.Get(entityId).Id);
        }
    }
}
