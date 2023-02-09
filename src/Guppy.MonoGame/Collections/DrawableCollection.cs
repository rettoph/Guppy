using Guppy.Common.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Collections
{
    public class DrawableCollection : DirtyableCollection<IDrawable>
    {
        protected override IEnumerable<IDrawable> Clean(IEnumerable<IDrawable> items)
        {
            return items.Where(x => x.Visible).OrderBy(x => x.DrawOrder);
        }

        public override void Add(IDrawable item)
        {
            base.Add(item);

            item.DrawOrderChanged += this.HandleDrawOrderChanged;
            item.VisibleChanged += this.HandleVisibleChanged;
        }

        public override bool Remove(IDrawable item)
        {
            if (base.Remove(item))
            {
                item.DrawOrderChanged -= this.HandleDrawOrderChanged;
                item.VisibleChanged -= this.HandleVisibleChanged;

                return true;
            }

            return false;
        }

        private void HandleDrawOrderChanged(object? sender, EventArgs e)
        {
            this.dirty = true;
        }

        private void HandleVisibleChanged(object? sender, EventArgs e)
        {
            this.dirty = true;
        }
    }
}
