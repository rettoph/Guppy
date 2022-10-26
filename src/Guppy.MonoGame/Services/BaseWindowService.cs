using Guppy.Common;
using Guppy.MonoGame.Messages.Inputs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal abstract class BaseWindowService : ISubscriber<ToggleWindowInput>
    {
        private bool _visible;
        private IGlobal<IBus> _bus;

        public abstract ToggleWindowInput.Windows Window { get; }

        public BaseWindowService(IGlobal<IBus> bus, bool visible)
        {
            _visible = visible;
            _bus = bus;

            _bus.Instance.Subscribe(this);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if(_visible)
            {
                this.InnerDraw(gameTime);
            }
        }

        protected abstract void InnerDraw(GameTime gameTime);

        public virtual void Update(GameTime gameTime)
        {

        }

        public void Process(in ToggleWindowInput message)
        {
            if(message.Window == this.Window)
            {
                _visible = !_visible;
            }
        }
    }
}
