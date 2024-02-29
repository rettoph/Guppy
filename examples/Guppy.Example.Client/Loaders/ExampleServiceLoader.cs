using Autofac;
using Guppy.Attributes;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Guppy.Loaders;

namespace Guppy.Example.Client.Loaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Camera2D>().As<Camera>().AsSelf().SingleInstance();
        }
    }
}
