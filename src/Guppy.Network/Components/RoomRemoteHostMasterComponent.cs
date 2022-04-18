using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class RoomRemoteHostMasterComponent : Component
    {
        public readonly Room Room;

        public RoomRemoteHostMasterComponent(Room room)
        {
            this.Room = room;
        }
    }
}
