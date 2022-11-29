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
        public void Draw(GameTime gameTime)
        {
            this.EnsureClean();

            foreach (IDrawable drawable in this.items)
            {
                drawable.Draw(gameTime);
            }
        }

        protected override IEnumerable<IDrawable> Clean(IEnumerable<IDrawable> items)
        {
            return items.Where(x => x.Visible).OrderBy(x => x.DrawOrder);
        }

        protected override bool Add(IDrawable item)
        {
            if(base.Add(item))
            {
                item.DrawOrderChanged += this.HandleDrawOrderChanged;
                item.VisibleChanged += this.HandleVisibleChanged;

                return true;
            }

            return false;
        }

        protected override bool Remove(IDrawable item)
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
