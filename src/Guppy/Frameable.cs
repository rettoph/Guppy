using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace Guppy
{
    public abstract class Frameable : Entity, IFrameable
    {
        #region Private Fields
        private Boolean _visible;
        private Boolean _enabled;

        private Action<GameTime> _update;
        private Action<GameTime> _draw;
        #endregion

        #region Public Properties
        public Boolean Visible
        {
            get => _visible;
            set
            {
                if(this.OnVisibleChanged.InvokeIf(value != _visible, this, ref _visible, value))
                {
                    _draw = value ? this.Draw_Visible : this.Draw_NotVisible;
                }
            }
        }
        public Boolean Enabled
        {
            get => _enabled;
            set
            {
                if (this.OnEnabledChanged.InvokeIf(value != _enabled, this, ref _enabled, value))
                {
                    _update = value ? this.Update_Enabled : this.Update_NotEnabled;
                }
            }
        }
        #endregion

        #region Events
        /// <inheritdoc/>
        public event Step OnPreDraw;
        /// <inheritdoc/>
        public event Step OnDraw;
        /// <inheritdoc/>
        public event Step OnPostDraw;
        /// <inheritdoc/>
        public event Step OnPreUpdate;
        /// <inheritdoc/>
        public event Step OnUpdate;
        /// <inheritdoc/>
        public event Step OnPostUpdate;

        public event OnEventDelegate<IFrameable, Boolean> OnVisibleChanged;
        public event OnEventDelegate<IFrameable, Boolean> OnEnabledChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Enabled = true;
            this.Visible = true;
        }
        #endregion

        #region Frame Methods
        /// <inheritdoc/>
        public virtual void TryDraw(GameTime gameTime)
        {
            _draw(gameTime);
        }

        /// <inheritdoc/>
        public virtual void TryUpdate(GameTime gameTime)
        {
            _update(gameTime);
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

        private void Update_Enabled(GameTime gameTime)
        {
            this.PreUpdate(gameTime);
            this.OnPreUpdate?.Invoke(gameTime);

            this.Update(gameTime);
            this.OnUpdate?.Invoke(gameTime);

            this.PostUpdate(gameTime);
            this.OnPostUpdate?.Invoke(gameTime);
        }

        private void Update_NotEnabled(GameTime gameTime)
        {
            //
        }

        private void Draw_Visible(GameTime gameTime)
        {
            this.PreDraw(gameTime);
            this.OnPreDraw?.Invoke(gameTime);

            this.Draw(gameTime);
            this.OnDraw?.Invoke(gameTime);

            this.PostDraw(gameTime);
            this.OnPostDraw?.Invoke(gameTime);
        }

        private void Draw_NotVisible(GameTime gameTime)
        {
            //
        }
        #endregion
    }
}
