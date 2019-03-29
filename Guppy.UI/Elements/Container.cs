using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    public abstract class Container : Element
    {
        protected List<Element> children;

        public Container(Unit x, Unit y, Unit width, Unit height) : base(x, y, width, height)
        {
            this.children = new List<Element>();
        }

        #region Adders & Removers
        public virtual TELement Add<TELement>(TELement child)
            where TELement : Element
        {
            this.Dirty = true;

            if (child.Parent != null)
                throw new Exception("Unable to add element to container. Element already has a parent!");

            this.children.Add(child);
            child.Parent = this;

            // Update the childs cache
            child.UpdateCache();

            return child;
        }

        public virtual Element Remove(Element child)
        {
            this.Dirty = true;

            if (!this.children.Contains(child))
                throw new Exception("Unable to remove element from container. Element is not a child of current container!");

            children.Remove(child);
            child.Parent = null;

            // Update the childs cache
            child.UpdateCache();

            return child;
        }
        #endregion

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Element child in this.children)
                child.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (Element child in this.children)
                child.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Methods
        protected internal override void UpdateCache()
        {
            base.UpdateCache();

            foreach (Element child in this.children)
                child.UpdateCache();
        }

        protected internal override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            foreach (Element child in this.children)
                child.AddDebugVertices(ref vertices);
        }
        #endregion
    }
}
