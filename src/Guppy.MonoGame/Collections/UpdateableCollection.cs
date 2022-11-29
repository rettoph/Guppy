using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Collections
{
    public class UpdateableCollection : DirtyableCollection<IUpdateable>
    {
        public void Update(GameTime gameTime)
        {
            this.EnsureClean();

            foreach (IUpdateable updatable in this.items)
            {
                updatable.Update(gameTime);
            }
        }

        protected override IEnumerable<IUpdateable> Clean(IEnumerable<IUpdateable> items)
        {
            return items.Where(x => x.Enabled).OrderBy(x => x.UpdateOrder);
        }

        protected override bool Add(IUpdateable item)
        {
            if (base.Add(item))
            {
                item.UpdateOrderChanged += this.HandleUpdateOrderChanged;
                item.EnabledChanged += this.HandleEnabledChanged;

                return true;
            }

            return false;
        }

        protected override bool Remove(IUpdateable item)
        {
            if (base.Remove(item))
            {
                item.UpdateOrderChanged -= this.HandleUpdateOrderChanged;
                item.EnabledChanged -= this.HandleEnabledChanged;

                return true;
            }

            return false;
        }

        private void HandleUpdateOrderChanged(object? sender, EventArgs e)
        {
            this.dirty = true;
        }

        private void HandleEnabledChanged(object? sender, EventArgs e)
        {
            this.dirty = true;
        }
    }
}
