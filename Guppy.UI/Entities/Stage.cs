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
    public class Stage : Entity
    {
        #region Private Fields
        private SpriteBatch _spriteBatch;
        private GameWindow _window;
        private Rectangle _innerBounds;

        private Boolean _dirtyBounds;

        private Matrix _projection;
        private Matrix _world;
        private Matrix _view;

        private BasicEffect _effect;

        private VertexBuffer _vertexBuffer;
        private Int32 _primitives;

        private ContentLoader _content;
        #endregion

        #region Protected Fields
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
            _window = window;
            _dirtyBounds = true;
            _innerBounds = new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            _content = provider.GetLoader<ContentLoader>();

            this.graphicsDevice = graphicsDevice;
            this.internalSpriteBatch = new SpriteBatch(this.graphicsDevice);
            this.inputManager = inputManager;
            this.StyleSheet = new StyleSheet();

            this.Content = new StageContainer(this, 0, 0, 1f, 1f);
            this.Content.UpdateBounds(_innerBounds);

            _window.ClientSizeChanged += this.HandleClientSizeChanged;

            // Setup debug view values...
            _view = Matrix.Identity;
            _world = Matrix.CreateTranslation(0, 0, 0);

            _effect = new BasicEffect(this.graphicsDevice);
            _effect.VertexColorEnabled = true;

            this.SetEnabled(true);
            this.SetVisible(true);
            this.Debug = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.StyleSheet.SetProperty(StyleProperty.Font, _content.Get<SpriteFont>("ui:font"));
            this.StyleSheet.SetProperty(StyleProperty.FontColor, Color.Black);
            this.StyleSheet.SetProperty(StyleProperty.TextAlignment, Alignment.CenterCenter);
            this.StyleSheet.SetProperty(StyleProperty.PaddingTop, (Unit)5);
            this.StyleSheet.SetProperty(StyleProperty.PaddingRight, (Unit)5);
            this.StyleSheet.SetProperty(StyleProperty.PaddingBottom, (Unit)5);
            this.StyleSheet.SetProperty(StyleProperty.PaddingLeft, (Unit)5);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the content
            this.Content.Draw(gameTime, _spriteBatch);

            if(this.Debug && _primitives > 0)
            { // Draw the debug overlay...
                this.graphicsDevice.SetVertexBuffer(_vertexBuffer);

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _primitives);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update the content
            this.Content.Update(gameTime);

            if(_dirtyBounds)
            { // Update the content if the elements bounds are dirty
                _innerBounds.Width = _window.ClientBounds.Width - 1;
                _innerBounds.Height = _window.ClientBounds.Height - 1;

                this.Content.UpdateBounds(_innerBounds);
                this.Content.UpdateCache();

                // Update debug projection settings...
                _projection = Matrix.CreateOrthographicOffCenter(0, _window.ClientBounds.Width, _window.ClientBounds.Height, 0, 0, 1);
                _effect.Projection = _projection;

                _dirtyBounds = false;
            }

            if(this.Debug)
            { // Update debug overlay settings...
                List<VertexPositionColor> _allVertices = new List<VertexPositionColor>();
                this.Content.RegisterDebugVertices(ref _allVertices);

                _vertexBuffer?.Dispose();

                _vertexBuffer = new VertexBuffer(this.graphicsDevice, typeof(VertexPositionColor), _allVertices.Count, BufferUsage.WriteOnly);
                _vertexBuffer.SetData<VertexPositionColor>(_allVertices.ToArray());

                _primitives = _allVertices.Count / 2;
            }
        }

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            _dirtyBounds = true;
        }
        #endregion
    }
}
