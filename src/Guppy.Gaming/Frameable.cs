using Guppy.EntityComponent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public abstract class Frameable : Entity, IFrameable
    {
        private bool _enabled = true;
        private bool _visible = true;

        public bool Enabled
        {
            get => _enabled;
            set => this.OnEnabledChanged.InvokeIf(value != _enabled, this, ref _enabled, value);
        }

        public bool Visible
        {
            get => _visible;
            set => this.OnVisibleChanged.InvokeIf(value != _visible, this, ref _visible, value);
        }

        public event OnEventDelegate<IFrameable, bool>? OnEnabledChanged;
        public event OnEventDelegate<IFrameable, bool>? OnVisibleChanged;

        public virtual void Draw(GameTime gameTime)
        {
            //
        }

        public virtual void Update(GameTime gameTime)
        {
            //
        }
    }
}
