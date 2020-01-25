using Guppy.Collections;
using Guppy.UI.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class Stage : BaseElement
    {
        #region Private Fields
        private GameWindow _window;
        private Rectangle _viewport;
        private PrimitiveBatch _defaultPrimitiveBatch;
        private SpriteBatch _defaultSpriteBatch;
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Internal Fields
        /// <summary>
        /// All pointer buttons that have been pressed since last frame
        /// </summary>
        internal Pointer.Button pressed { get; private set; }
        /// <summary>
        /// All pointer buttons that have been released since last frame
        /// </summary>
        internal Pointer.Button released { get; private set; }
        #endregion

        #region Protected Fields
        protected override Stage stage => this;

        protected override PrimitiveBatch primitiveBatch => _primitiveBatch;

        protected override SpriteBatch spriteBatch => _spriteBatch;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _window = provider.GetRequiredService<GameWindow>();
            _viewport = new Rectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);

            _defaultPrimitiveBatch = provider.GetRequiredService<PrimitiveBatch>();
            _defaultSpriteBatch = provider.GetRequiredService<SpriteBatch>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.SetEnabled(true);
            this.SetVisible(true);

            // Disable events on the stage by default
            this.EventType = EventTypes.None;

            _window.ClientSizeChanged += this.HandleClientSizeChanged;

            this.pointer.OnPressed += this.HandleButtonPressed;
            this.pointer.OnReleased += this.HandleButtonReleased;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
        }
        #endregion

        #region Frame Methods
        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            // Reset frame updates..
            this.pressed = 0;
            this.released = 0;
        }

        protected override void PreDraw(GameTime gameTIme)
        {
            base.PreDraw(gameTIme);

            this.ResetBatchs();
        }
        #endregion

        #region Methods
        protected internal override Rectangle GetParentBounds()
        {
            return _viewport;
        }

        /// <summary>
        /// Set a custom spritebatch (for the remainder of this frame)
        /// Starting next frame the batches will be restored to their
        /// defaults.
        /// </summary>
        public void SetBatches(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            this.SetSpriteBatch(spriteBatch);
            this.SetPrimitiveBatch(primitiveBatch);
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void SetPrimitiveBatch(PrimitiveBatch primitiveBatch)
        {
            _primitiveBatch = primitiveBatch;
        }

        /// <summary>
        /// Reset the primitivebatch &
        /// & spritebatchs to their default
        /// values
        /// </summary>
        public void ResetBatchs()
        {
            this.SetBatches(_defaultSpriteBatch, _defaultPrimitiveBatch);
        }
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.dirty = true;
            _viewport = new Rectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
        }

        private void HandleButtonPressed(object sender, Pointer.Button e)
        {
            this.pressed |= e;
        }

        private void HandleButtonReleased(object sender, Pointer.Button e)
        {
            this.released |= e;
        }
        #endregion
    }
}
