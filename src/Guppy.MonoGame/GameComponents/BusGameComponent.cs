using Guppy.Attributes;
using Guppy.Common;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.GameComponents
{
    internal sealed class BusGameComponent : SimpleGameComponent
    {
        private IBus _bus;

        public BusGameComponent(IBus bus)
        {
            _bus = bus;
        }

        public override void Initialize()
        {
            _bus.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
