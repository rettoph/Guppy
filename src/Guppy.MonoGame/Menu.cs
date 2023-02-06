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
        private IList<MenuItem> _items;

        public readonly string Name;

        public ReadOnlyCollection<MenuItem> Items;

        public Menu(string name)
        {
            _items = new List<MenuItem>();

            this.Name = name;

            this.Items = new ReadOnlyCollection<MenuItem>(_items);
        }

        public void Add(MenuItem item)
        {
            _items.Add(item);
        }

        public Menu Add(params MenuItem[] items)
        {
            foreach(var item in items)
            {
                this.Add(item);
            }

            return this;
        }
    }
}
