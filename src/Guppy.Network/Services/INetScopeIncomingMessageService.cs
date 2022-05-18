using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface INetScopeIncomingMessageService
    {
        void Enqueue(NetIncomingMessage message);
        void Read();
    }
}
