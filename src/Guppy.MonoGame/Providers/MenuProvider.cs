using Guppy.Common;
using Guppy.MonoGame.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Providers
{
    internal sealed class MenuProvider : IMenuProvider
    {
        private readonly IDictionary<string, Menu> _menus;

        public MenuProvider(IEnumerable<Menu> menus, IEnumerable<IMenuLoader> loaders)
        {
            _menus = menus.ToDictionary(x => x.Name, x => x);

            foreach(var loader in loaders)
            {
                loader.Load(this);
            }
        }

        public void Add(Menu menu)
        {
            _menus.Add(menu.Name, menu);
        }

        public Menu Get(string name)
        {
            return _menus[name];
        }
    }
}
