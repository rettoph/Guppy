using Guppy.Network;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public sealed class PaddleTargetDto : IPacket
    {
        public readonly Single Target;

        public PaddleTargetDto(float target)
        {
            Target = target;
        }

        internal static PaddleTargetDto Read(NetDataReader arg1, NetworkProvider arg2)
        {
            return new PaddleTargetDto(arg1.GetFloat());
        }

        internal static void Write(NetDataWriter arg1, NetworkProvider arg2, PaddleTargetDto arg3)
        {
            arg1.Put(arg3.Target);
        }
    }
}
