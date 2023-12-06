using Guppy.Common;
using Guppy.Game.MonoGame.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame.Providers
{
    internal sealed class MenuProvider : IMenuProvider
    {
        private readonly IDictionary<string, Menu> _menus;

        public MenuProvider(IEnumerable<Menu> menus)
        {
            _menus = menus.ToDictionary(x => x.Name, x => x);
        }

        public void Add(Menu menu)
        {
            _menus.Add(menu.Name, menu);
        }

        public Menu Get(string name)
        {
            if (!_menus.TryGetValue(name, out var menu))
            {
                menu = new Menu(name);
                _menus.Add(name, menu);
            }

            return menu;
        }
    }
}
