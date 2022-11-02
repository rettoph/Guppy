using Guppy.Common;
using Guppy.ECS.Attributes;
using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Systems
{
    [SingletonFilter]
    internal sealed class GlobalSystem : IUpdateSystem, IDrawSystem
    {
        private IGlobal<IBus> _bus;

        public GlobalSystem(IGlobal<IBus> bus)
        {
            _bus = bus;
        }

        public void Initialize(World world)
        {
            // throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            _bus.Instance.Flush();
        }

        public void Draw(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
