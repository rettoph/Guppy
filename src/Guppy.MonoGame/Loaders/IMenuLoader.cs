using Guppy.Common.Attributes;
using Guppy.Loaders;
using Guppy.MonoGame.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    [Scoped<IMenuLoader>()]
    public interface IMenuLoader
    {
        void Load(IMenuProvider menus);
    }
}
