using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Elements
{
    public class ScrollItems : Container
    {
        private ScrollContainer _container;

        public ScrollItems(ScrollContainer container) : base(0, -15, 1f, 1f)
        {
            _container = container;
            this.Inner.X.SetValue(0);
            this.Inner.Y.SetValue(0);
            this.Inner.Width.SetValue(1f);
            this.Inner.Height.SetValue(1f);
        }

        public override void Update(MouseState mouse)
        {
            base.Update(mouse);
        }

        public override void Add(Element child)
        {
            base.Add(child);

            this.Outer.Height.SetValue(this.children.Max(e => e.Outer.RelativeBounds.Bottom));
        }

        public override void Remove(Element child)
        {
            base.Add(child);

            this.Outer.Height.SetValue(this.children.Max(e => e.Outer.RelativeBounds.Bottom));
        }
    }
}
