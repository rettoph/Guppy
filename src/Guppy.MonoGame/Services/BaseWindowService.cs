using Guppy.Common;
using Guppy.MonoGame.Commands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal abstract class BaseWindowService : ISubscriber<ToggleWindow>
    {
        private bool _visible;
        private ICommandService _commands;

        public abstract ToggleWindow.Windows Window { get; }

        public BaseWindowService(ICommandService commands, bool visible)
        {
            _visible = visible;
            _commands = commands;

            _commands.Subscribe(this);
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

        public void Process(in ToggleWindow message)
        {
            if(message.Window == this.Window)
            {
                _visible = !_visible;
            }
        }
    }
}
