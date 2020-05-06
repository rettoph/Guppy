using Guppy.DependencyInjection;
using Guppy.UI.Entities;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class TextButton : TextComponent, IButton
    {
        #region Events
        public event EventHandler OnClicked;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.OnButtonReleased += this.HandleButtonReleased;
        }
        #endregion

        #region Event Handlers
        private void HandleButtonReleased(object sender, Cursor.Button e)
        {
            if (this.Hovered && e == Cursor.Button.Left)
                this.OnClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
