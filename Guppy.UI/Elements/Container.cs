using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Guppy.UI.Styles;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Represents a generic element that can
    /// can contain several smaller elements.
    /// </summary>
    public abstract class Container : Element
    {
        private List<Element> _children;
        public ReadOnlyCollection<Element> Children { get; private set; }

        protected Container(Style style = null) : base(style)
        {
            _children = new List<Element>();
        }
        public Container(UnitValue x, UnitValue y, UnitValue width, UnitValue height, Style style = null) : base(x, y, width, height, style)
        {
            _children = new List<Element>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Element child in _children)
                child.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Element child in _children)
                child.Draw(spriteBatch);
        }

        protected void add(Element child)
        {
            if (child.Container != null)
                throw new Exception("Child already belongs to another container!");

            _children.Add(child);
            child.setContainer(this);
            this.DirtyMeta = true;
        }
        protected void remove(Element child)
        {
            _children.Remove(child);
            child.setContainer(null);
            this.DirtyMeta = true;
        }

        protected override void CleanMeta()
        {
            base.CleanMeta();

            this.Children = _children.AsReadOnly();
        }
        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            foreach (Element child in _children.OrderBy(e => e.State))
                child.AddDebugVertices(ref vertices);
        }
    }
}
