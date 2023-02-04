using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public class Menu
    {
        private IList<IMenuItem> _items;

        public readonly string Name;

        public ReadOnlyCollection<IMenuItem> Items;

        public Menu(string name)
        {
            _items = new List<IMenuItem>();

            this.Name = name;

            this.Items = new ReadOnlyCollection<IMenuItem>(_items);
        }

        public void Add(IMenuItem item)
        {
            _items.Add(item);
        }

        public Menu Add(params IMenuItem[] items)
        {
            foreach(var item in items)
            {
                this.Add(item);
            }

            return this;
        }
    }
}
