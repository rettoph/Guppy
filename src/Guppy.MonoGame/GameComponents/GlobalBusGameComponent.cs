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
    [GlobalScopeFilter]
    internal sealed class GlobalBusGameComponent : SimpleGameComponent
    {
        private IGlobal<IBus> _bus;

        public GlobalBusGameComponent(IGlobal<IBus> bus)
        {
            _bus = bus;
        }

        public override void Initialize()
        {
            _bus.Instance.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _bus.Instance.Flush();
        }
    }
}
