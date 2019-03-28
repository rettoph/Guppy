using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Containers are specific elements that can contain
    /// other elements.
    /// </summary>
    public class Container : Element
    {
        private List<Element> _children;

        public Container(Unit x, Unit y, Unit width, Unit height) : base(x, y, width, height)
        {
            _children = new List<Element>();
        }

        #region Adders & Removers
        public Element Add(Element child)
        {
            if (child.Parent != null)
                throw new Exception("Unable to add element to container. Element already has a parent!");

            _children.Add(child);
            child.Parent = this;
            child.Stage = this.Stage;

            // Update the childs cache
            child.UpdateCache();

            return child;
        }

        public Element Remove(Element child)
        {
            if (!_children.Contains(child))
                throw new Exception("Unable to remove element from container. Element is not a child of current container!");

            _children.Remove(child);
            child.Parent = null;
            child.Stage = null;

            // Update the childs cache
            child.UpdateCache();

            return child;
        }
        #endregion

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Element child in _children)
                child.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (Element child in _children)
                child.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Methods
        protected internal override void UpdateCache()
        {
            base.UpdateCache();

            foreach (Element child in _children)
                child.UpdateCache();
        }

        protected internal override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            foreach (Element child in _children)
                child.AddDebugVertices(ref vertices);
        }
        #endregion
    }
}
