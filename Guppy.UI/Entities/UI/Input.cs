using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Entities.UI
{
    public abstract class Input : StyleElement
    {
        #region Private Fields
        private Boolean _active;
        #endregion

        #region Public Attributes 
        public Boolean Active
        {
            get => _active;
            internal set {
                if(_active != value)
                {
                    _active = value;
                    this.OnActiveChanged?.Invoke(this, this.Active);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<Boolean> OnActiveChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.EventType = EventTypes.Normal;

            this.OnButtonReleased += this.HandleButtonReleased;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.OnButtonReleased -= this.HandleButtonReleased;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Unset Active if applicable...
            if (this.Active && !this.Hovered && (this.stage.pressed & Pointer.Button.Left) != 0)
                this.Active = false;
        }
        #endregion

        #region Event Handlers
        private void HandleButtonReleased(object sender, Pointer.Button e)
        {
            // Set Active if applicable...
            if(!this.Active && e == Pointer.Button.Left && this.Hovered)
                this.Active = true;
        }
        #endregion
    }
}
