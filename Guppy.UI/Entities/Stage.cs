using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.UI.Elements;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    /// <summary>
    /// The stage is a root container for a UI.
    /// All buttons, containers, inputs, and other
    /// elements must reside within a stage.
    /// </summary>
    public class Stage : DebuggableEntity
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private Rectangle _innerBounds;

        private Boolean _dirtyBounds;

        private ContentLoader _content;
        #endregion

        #region Protected Fields
        protected internal GameWindow window;
        protected internal SpriteBatch internalSpriteBatch;
        protected internal GraphicsDevice graphicsDevice;
        protected internal InputManager inputManager;
        #endregion

        #region Public Attributes
        /// <summary>
        /// When true, a wireframe of the stage and all contained elements
        /// will be rendered on draw.
        /// </summary>
        public Boolean Debug { get; set; }

        /// <summary>
        /// The stage's main container to hold all
        /// content elements.
        /// </summary>
        public Container Content { get; set; }

        public StyleSheet StyleSheet { get; private set; }
        #endregion

        public Stage(IServiceProvider provider, InputManager inputManager, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameWindow window, EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            _spriteBatch = spriteBatch;
            this.window = window;
            _dirtyBounds = true;
            _innerBounds = new Rectangle(0, 0, this.window.ClientBounds.Width, this.window.ClientBounds.Height);
            _content = provider.GetLoader<ContentLoader>();

            this.graphicsDevice = graphicsDevice;
            this.internalSpriteBatch = new SpriteBatch(this.graphicsDevice);
            this.inputManager = inputManager;
            this.StyleSheet = new StyleSheet();

            this.Content = new StageContainer(this, 0, 0, 1f, 1f);
            this.Content.UpdateBounds(_innerBounds);

            this.window.ClientSizeChanged += this.HandleClientSizeChanged;

            this.SetEnabled(true);
            this.SetVisible(true);
            this.Debug = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.StyleSheet.SetProperty<SpriteFont>(StyleProperty.Font, _content.Get<SpriteFont>("ui:font"));
            this.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Black);
            this.StyleSheet.SetProperty<Alignment>(StyleProperty.TextAlignment, Alignment.CenterCenter);
            this.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingTop, 5);
            this.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingRight, 5);
            this.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingBottom, 5);
            this.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingLeft, 5);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the content
            this.Content.Draw(gameTime, _spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // Update the content
            this.Content.Update(gameTime);

            if(_dirtyBounds)
            { // Update the content if the elements bounds are dirty
                _innerBounds.Width = this.window.ClientBounds.Width - 1;
                _innerBounds.Height = this.window.ClientBounds.Height - 1;

                this.Content.UpdateBounds(_innerBounds);
                this.Content.UpdateCache();

                _dirtyBounds = false;
            }
        }

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            _dirtyBounds = true;
        }

        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.Content.AddDebugVertices(ref vertices);
        }
        #endregion
    }
}
