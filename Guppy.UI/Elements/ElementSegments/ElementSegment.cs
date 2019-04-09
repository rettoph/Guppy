using Guppy.UI.Enums;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements.ElementSegments
{
    /// <summary>
    /// Element Segements represent a specific section
    /// of an element. Generally, these are used
    /// to represent the inner/outer brounder of an element
    /// </summary>
    public abstract class ElementSegment : UnitRectangle
    {
        #region Static Fields
        protected static ElementState[] ElementStates = ((ElementState[])Enum.GetValues(typeof(ElementState)));
        #endregion

        #region Private Fields
        private Rectangle _localBounds;
        private Dictionary<ElementState, List<VertexPositionColor>> _debugVertices;
        private Dictionary<ElementState, Texture2D> _textures;
        protected Element element;
        #endregion

        #region Public Fields
        public Boolean DirtyTextures { get; private set; }

        public Rectangle LocalBounds { get { return _localBounds; } }
        #endregion

        public ElementSegment(Element element) : base(0, 0, 0, 0, null)
        {
            this.element = element;
            _debugVertices = new Dictionary<ElementState, List<VertexPositionColor>>();
            _textures = new Dictionary<ElementState, Texture2D>();
            _localBounds = new Rectangle(0, 0, 0, 0);

            this.Width.OnUpdated += this.HandleSizeChanged;
            this.Height.OnUpdated += this.HandleSizeChanged;

            // Ensure that the textures get generated the first time...
            this.DirtyTextures = true;
        }
        #region Frame Methods
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(_textures.ContainsKey(this.element.State))
                spriteBatch.Draw(_textures[this.element.State], this.Bounds, Color.White);
        }

        public override void Update()
        {
            base.Update();

            // Clean the textures if they are dirty
            if(this.DirtyTextures)
            {
                this.CleanTextures();

                this.DirtyTextures = false;
            }
        }
        #endregion

        #region Cleaner Methods
        protected override void CleanBounds()
        {
            base.CleanBounds();

            // Ensure that the debug vertices get updated
            foreach(ElementState state in ElementSegment.ElementStates)
                if(!this.element.StateBlacklisted(state))
                { // If the current state isnt blacklisted we should regenerate the debug vertices
                    if (!_debugVertices.ContainsKey(state))
                        _debugVertices.Add(state, new List<VertexPositionColor>());
                    // Generate the new vertices
                    this.generateDebugVertices(state, _debugVertices[state]);
                }

            // Update the local bounds
            _localBounds.Width = this.Bounds.Width;
            _localBounds.Height = this.Bounds.Height;
        }

        protected virtual void CleanTextures()
        {
            // Alert the stage to switch to the internal render target
            this.element.stage.startInternalGraphics();

            // Ensure that the textures get updated
            if(this.LocalBounds.Width > 0 && this.LocalBounds.Height > 0)
                foreach (ElementState state in ElementSegment.ElementStates)
                    if (!this.element.StateBlacklisted(state))
                    { // If the current state isnt blacklisted we should regenerate the textures
                        // Ensure that the render target is cleared
                        this.element.stage.graphicsDevice.Clear(ClearOptions.Target, Color.Transparent, 0, 0);
                        // Call the texture generator
                        this.generateTexture(state, this.element.stage.internalSpriteBatch);

                        if(!_textures.ContainsKey(state))
                        { // Create a new texture if one doesnt already exist
                            _textures.Add(state, new Texture2D(this.element.stage.graphicsDevice, this.LocalBounds.Width, this.LocalBounds.Height));
                        }
                        else if(this.LocalBounds.Width != _textures[state].Width || this.LocalBounds.Height != _textures[state].Height)
                        { // If the texture bounds have changed..
                            _textures[state].Dispose();
                            _textures[state] = new Texture2D(this.element.stage.graphicsDevice, this.LocalBounds.Width, this.LocalBounds.Height);
                        }

                        // Grab the pixel data from our render target and transplant it onto the new texure object
                        var data = new Color[this.LocalBounds.Width * this.LocalBounds.Height];
                        this.element.stage.internalRenderTarget.GetData<Color>(0, this.LocalBounds, data, 0, data.Length);
                        _textures[state].SetData<Color>(data);
                    }
        }
        #endregion

        #region Generation Methods
        /// <summary>
        /// Regenerate any self contained debug vertices.
        /// This method is automatically called when the 
        /// base class UnitRectangle OnBoundsCleaned event
        /// is triggered. (See EventHandlers region)
        /// </summary>
        /// <param name="state"></param>
        /// <param name="vertices"></param>
        protected virtual void generateDebugVertices(ElementState state, List<VertexPositionColor> vertices)
        {
            vertices.Clear();

            var color = this.getDebugVerticeColor(state);

            // Add outer vertices...
            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), color));
        }
        #endregion

        #region Methods
        public virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            if(_debugVertices.ContainsKey(this.element.State))
                vertices.AddRange(_debugVertices[this.element.State]);
        }
        #endregion

        #region Abstract Methods
        public abstract Color getDebugVerticeColor(ElementState state);
        public abstract void generateTexture(ElementState state, SpriteBatch spriteBatch);
        #endregion

        #region Event Handlers
        /// <summary>
        /// Triggered when the width or height it updated.
        /// This will mark the current segment textures as dirty and ensure all
        /// textures get regenerated on next updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSizeChanged(object sender, Unit e)
        {
            this.DirtyTextures = true;
        }
        #endregion
    }
}
