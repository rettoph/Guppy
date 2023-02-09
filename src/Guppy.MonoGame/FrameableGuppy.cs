using Guppy.Common;
using Guppy.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public abstract class FrameableGuppy : IGuppy, IDrawable, IUpdateable
    {
        private int _drawOrder;
        private bool _visible;
        private bool _enabled;
        private int _updateOrder;

        public int DrawOrder
        {
            get => _drawOrder;
            set
            {
                _drawOrder = value;
                this.DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                this.VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                this.EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                _updateOrder = value;
                this.UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IBus Bus { get; private set; }
        public IGameComponentService Components { get; private set; }


        public FrameableGuppy()
        {
            this.Bus = default!;
            this.Components = default!;
            this.Visible = true;
            this.Enabled = true;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event OnEventDelegate<IDisposable>? OnDispose;

        public virtual void Initialize(IServiceProvider provider)
        {
            this.Components = provider.GetRequiredService<IGameComponentService>();
            this.Bus = provider.GetRequiredService<IBus>();

            this.Bus.Initialize();
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.Components.Draw(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Components.Update(gameTime);

            this.Bus.Flush();
        }

        public virtual void Dispose()
        {
            this.OnDispose?.Invoke(this);
        }
    }
}
