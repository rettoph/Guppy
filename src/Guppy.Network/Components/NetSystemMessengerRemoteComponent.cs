using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Entities;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    [HostTypeRequired(HostType.Remote)]
    internal sealed class NetSystemMessengerRemoteComponent : Component<NetSystemMessenger>
    {
        protected override void Initialize(NetSystemMessenger entity)
        {
            throw new NotImplementedException();
        }

        protected override void Uninitilaize()
        {
            throw new NotImplementedException();
        }
    }
}
