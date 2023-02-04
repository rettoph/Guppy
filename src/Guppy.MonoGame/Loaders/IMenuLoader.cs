using Guppy.Common.Attributes;
using Guppy.MonoGame.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.MonoGame.Loaders
{
    [Service<IMenuLoader>(ServiceLifetime.Scoped, true)]
    public interface IMenuLoader
    {
        void Load(IMenuProvider menus);
    }
}
