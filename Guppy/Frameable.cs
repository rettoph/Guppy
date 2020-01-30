using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Frameable : Initializable
    {
        #region Public Attributes
        public Boolean Visible { get; protected set; }
        public Boolean Enabled { get; protected set; }
        #endregion

        #region Events & Delegaters
        public event EventHandler<Boolean> OnVisibleChanged;
        public event EventHandler<Boolean> OnEnabledChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Visible = true;
            this.Enabled = true;
        }
        #endregion

        #region Frame Methods
        public virtual void TryDraw(GameTime gameTime)
        {
            this.PreDraw(gameTime);
            this.Draw(gameTime);
            this.PostDraw(gameTime);
        }

        public virtual void TryUpdate(GameTime gameTime)
        {
            this.PreUpdate(gameTime);
            this.Update(gameTime);
            this.PostUpdate(gameTime);
        }

        protected virtual void PreDraw(GameTime gameTime)
        {
            //
        }
        protected virtual void Draw(GameTime gameTime)
        {
            // 
        }
        protected virtual void PostDraw(GameTime gameTime)
        {
            //
        }

        protected virtual void PreUpdate(GameTime gameTime)
        {
            //
        }
        protected virtual void Update(GameTime gameTime)
        {
            // 
        }
        protected virtual void PostUpdate(GameTime gameTime)
        {
            //
        }
        #endregion

        #region Helper Methods
        public virtual void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.OnVisibleChanged?.Invoke(this, this.Visible);
            }
        }

        public virtual void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.OnEnabledChanged?.Invoke(this, this.Enabled);
            }
        }
        #endregion
    }
}
