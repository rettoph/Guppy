using Guppy.ECS;
using Guppy.Example.Library.Components;
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
    internal sealed class SlaveCreateNetworkedSystem : EntitySystem
    {
        private NetScope _scope;
        private World _world;

        public SlaveCreateNetworkedSystem(NetScope scope, World world) : base(Aspect.All(typeof(EntityType), typeof(Networked)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            // throw new NotImplementedException();
        }
    }
}
