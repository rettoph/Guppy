using Guppy.Common;
using Guppy.Messaging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame
{
    public sealed class MenuItem
    {
        public required string Label { get; init; }
        public required IMessage OnClick { get; init; }

        public MenuItem()
        {

        }
        public MenuItem(string label, IMessage onClick)
        {
            this.Label = label;
            this.OnClick = onClick;
        }
    }
}
