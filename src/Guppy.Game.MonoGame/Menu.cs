using System.Collections.ObjectModel;

namespace Guppy.Game.MonoGame
{
    public class Menu
    {
        private IList<MenuItem> _items;

        public readonly string Name;

        public ReadOnlyCollection<MenuItem> Items;

        public event OnEventDelegate<Menu, MenuItem>? OnItemAdded;

        public Menu(string name)
        {
            _items = new List<MenuItem>();

            this.Name = name;

            this.Items = new ReadOnlyCollection<MenuItem>(_items);
        }

        public void Add(MenuItem item)
        {
            _items.Add(item);
            this.OnItemAdded?.Invoke(this, item);
        }

        public Menu Add(params MenuItem[] items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }

            return this;
        }
    }
}
