using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Frameable : Initializable, IFrameable
    {
        #region Private Fields
        private Boolean _visible;
        private Boolean _enabled;
        #endregion

        #region Public Attributes
        public Boolean Visible
        {
            get => _visible;
            set
            {
                if (value != this.Visible)
                {
                    _visible = value;

                    this.OnVisibleChanged?.Invoke(this, this.Visible);
                }
            }
        }
        public Boolean Enabled
        {
            get => _enabled;
            set
            {
                if (value != this.Enabled)
                {
                    _enabled = value;

                    this.OnEnabledChanged?.Invoke(this, this.Enabled);
                }
            }
        }
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
    }
}
