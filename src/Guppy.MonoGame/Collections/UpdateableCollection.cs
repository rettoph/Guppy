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
        protected override IEnumerable<IUpdateable> Clean(IEnumerable<IUpdateable> items)
        {
            return items.Where(x => x.Enabled).OrderBy(x => x.UpdateOrder);
        }

        public override void Add(IUpdateable item)
        {
            base.Add(item);

            item.UpdateOrderChanged += this.HandleUpdateOrderChanged;
            item.EnabledChanged += this.HandleEnabledChanged;
        }

        public override bool Remove(IUpdateable item)
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
