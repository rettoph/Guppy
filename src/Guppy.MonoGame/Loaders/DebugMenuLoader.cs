using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class DebugMenuLoader : IGuppyLoader
    {
        private readonly IMenuProvider _menus;

        public DebugMenuLoader(IMenuProvider menus)
        {
            _menus = menus;
        }

        public void Load(IGuppy guppy)
        {
            _menus.Add(new Menu(MenuConstants.Debug));
        }
    }
}
