using Guppy.Attributes;
using Guppy.MonoGame.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    [Service<IMenuLoader>(ServiceLifetime.Scoped, true)]
    public interface IMenuLoader
    {
        void Load(IMenuProvider menus);
    }
}
