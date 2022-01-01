using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.EventArgs
{
    public sealed class MagicNetworkEntityPipeEventArgs : System.EventArgs, IData
    {
        public readonly Pipe NewPipe;
        public readonly Pipe OldPipe;
        public readonly IMagicNetworkEntity Entity;

        public MagicNetworkEntityPipeEventArgs(Pipe newPipe, Pipe oldPipe, IMagicNetworkEntity entity)
        {
            this.NewPipe = newPipe;
            this.OldPipe = oldPipe;
            this.Entity = entity;
        }
    }
}
