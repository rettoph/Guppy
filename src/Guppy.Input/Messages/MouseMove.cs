using Guppy.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Messages
{
    public sealed class MouseMove : Message<MouseMove>
    {
        public readonly Vector2 Position;
        public readonly Vector2 Delta;

        public MouseMove(Vector2 position, MouseMove? previous)
        {
            this.Position = position;
            this.Delta = previous is null ? this.Position : this.Position - previous.Position;
        }
    }
}
