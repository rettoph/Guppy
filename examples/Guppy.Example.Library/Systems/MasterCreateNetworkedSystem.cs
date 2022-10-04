using Guppy.Attributes;
using Guppy.ECS;
using Guppy.Example.Library.Components;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Systems
{
    [AutoLoad]
    internal class MasterCreateNetworkedSystem : EntitySystem
    {
        private NetScope _scope;
        private ComponentMapper<EntityType>? _typeMapper;

        public MasterCreateNetworkedSystem(NetScope scope) : base(Aspect.All(typeof(EntityType), typeof(Networked)))
        {
            _scope = scope;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _typeMapper = mapperService.GetMapper<EntityType>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            base.OnEntityAdded(entityId);

            _scope.Create<CreateEntity>(new CreateEntity(_typeMapper!.Get(entityId).Key.Id))
                .AddRecipients(_scope.Users.Peers)
                .Enqueue();
        }
    }
}
