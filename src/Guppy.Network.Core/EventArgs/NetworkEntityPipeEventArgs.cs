using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.EventArgs
{
    public sealed class NetworkEntityPipeEventArgs : System.EventArgs, IMessage
    {
        public readonly Pipe NewPipe;
        public readonly Pipe OldPipe;
        public readonly INetworkEntity Entity;

        public NetworkEntityPipeEventArgs(Pipe newPipe, Pipe oldPipe, INetworkEntity entity)
        {
            this.NewPipe = newPipe;
            this.OldPipe = oldPipe;
            this.Entity = entity;
        }
    }
}
