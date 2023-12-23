using Autofac;
using System.Reflection;

namespace Guppy.Game.Common.Extensions
{
    public static class GuppyEngineExtensions
    {
        public static IGame StartGame(
            this GuppyEngine engine,
            Action<ContainerBuilder>? build = null,
            Assembly? entry = null)
        {
            return engine.Start(build, entry).Scope.Resolve<IGame>();
        }
    }
}
