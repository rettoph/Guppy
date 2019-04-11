using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.UI.Elements;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Stage : DebuggableEntity
    {
        #region Private Fields
        private GameWindow _window;
        #endregion

        #region Protected Internal Fields
        /// <summary>
        /// Any elements containing dirty textures will
        /// be added to this queue. The stage will then
        /// manage the cleaning of all dirty textures post
        /// update.
        /// </summary>
        protected internal Queue<Element> dirtyTextureQueue;

        /// <summary>
        /// The bounds of the current client window
        /// </summary>
        protected internal UnitRectangle clientBounds { get; private set; }
        #endregion

        #region Public Attributes
        public Container Content { get; private set; }
        #endregion

        #region Constructors
        public Stage(
            SpriteBatch spriteBatch,
            GameWindow window,
            GraphicsDevice graphicsDevice,
            EntityConfiguration configuration,
            Scene scene,
            IServiceProvider provider,
            ILogger logger) : base(configuration, scene, logger)
        {
            _window = window;

            this.clientBounds = new UnitRectangle(0, 0, _window.ClientBounds.Width - 1, _window.ClientBounds.Height - 1);
            this.dirtyTextureQueue = new Queue<Element>();

            var style = new Style();
            style.Set<UnitValue>(GlobalProperty.PaddingTop, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingRight, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingBottom, 15);
            style.Set<UnitValue>(GlobalProperty.PaddingLeft, 15);

            this.Content = new Container(0, 0, 1f, 1f, style);
            this.Content.Outer.setParent(this.clientBounds);

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;

            this.Content.Add(new Element(10, 10, 100, 100));
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            this.Content.Update();
        }
        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            this.Content.AddDebugVertices(ref vertices);
        }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.clientBounds.Width.SetValue(_window.ClientBounds.Width - 1);
            this.clientBounds.Height.SetValue(_window.ClientBounds.Height - 1);

            this.clientBounds.Update();
        }
        #endregion
    }
}
