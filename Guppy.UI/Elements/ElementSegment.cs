using Guppy.UI.Enums;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Element Segements represent a specific section
    /// of an element. Generally, these are used
    /// to represent the inner/outer brounder of an element
    /// </summary>
    public class ElementSegment : UnitRectangle
    {
        #region Static Fields
        protected static ElementState[] ElementStates = ((ElementState[])Enum.GetValues(typeof(ElementState)));
        #endregion

        #region Private Fields
        private Dictionary<ElementState, List<VertexPositionColor>> _debugVertices;
        private Dictionary<ElementState, Texture2D> _textures;
        private Element _parent;
        private StyleProperty _debugVerticeColor;
        #endregion

        public ElementSegment(Element parent, StyleProperty debugVerticeColor) : base(0, 0, 0, 0, null)
        {
            _parent = parent;
            _debugVerticeColor = debugVerticeColor;
            _debugVertices = new Dictionary<ElementState, List<VertexPositionColor>>();
            _textures = new Dictionary<ElementState, Texture2D>();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(_textures[_parent.State], this.Bounds, Color.White);
        }

        #region Cleaner Methods
        protected override void CleanBounds()
        {
            base.CleanBounds();

            // Ensure that the debug vertices get updated
            foreach(ElementState state in ElementSegment.ElementStates)
                if(!_parent.StateBlacklisted(state))
                { // If the current state isnt blacklisted we should regenerate the debug vertices
                    if (!_debugVertices.ContainsKey(state))
                        _debugVertices.Add(state, new List<VertexPositionColor>());
                    // Generate the new vertices
                    this.generateDebugVertices(state, _debugVertices[state]);
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

            var color = _parent.Style.Get<Color>(state, _debugVerticeColor);

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

        public virtual void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            if(_debugVertices.ContainsKey(_parent.State))
                vertices.AddRange(_debugVertices[_parent.State]);
        }

        #region Event Handlers
        #endregion
    }
}
