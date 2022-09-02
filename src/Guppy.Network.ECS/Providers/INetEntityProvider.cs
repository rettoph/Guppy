using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Providers
{
    public interface INetEntityProvider
    {
        Entity GetEntity(int id);
        Entity GetEntity(ushort netId);

        int GetId(ushort netId);
        ushort GetNetId(int id);
    }
}
