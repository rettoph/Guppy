using Guppy.MonoGame;
using Guppy.MonoGame.Services;
using Guppy.Network;
using Guppy.Network.Peers;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library
{
    public class ExampleGuppy : FrameableGuppy
    {
        protected readonly NetScope scope;

        public ExampleGuppy(NetScope scope, ITerminalService terminal, IDebuggerService debugger, World world) : base(terminal, debugger, world)
        {
            this.scope = scope;

            this.scope.Start(0);
        }
    }
}
