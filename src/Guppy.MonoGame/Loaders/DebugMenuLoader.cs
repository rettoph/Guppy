using Guppy.Common.Attributes;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    [Sortable<IMenuLoader>(int.MinValue)]
    internal sealed class DebugMenuLoader : IMenuLoader
    {
        public void Load(IMenuProvider menus)
        {
            menus.Add(new Menu(MenuConstants.Debug));
        }
    }
}
