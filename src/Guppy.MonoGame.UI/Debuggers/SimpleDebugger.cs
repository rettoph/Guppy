using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Debuggers
{
    public abstract class SimpleDebugger : IDebugger
    {
        private bool _isVisible = true;
        public bool Visible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool IDrawable.Visible => _isVisible;

        private int _drawOrder;
        public int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (_drawOrder == value)
                    return;

                _drawOrder = value;
                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public abstract void Draw(GameTime gameTime);

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool IUpdateable.Enabled => _isEnabled;

        private int _updateOrder;
        public int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (_updateOrder == value)
                    return;

                _updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public abstract void Update(GameTime gameTime);
    }
}
