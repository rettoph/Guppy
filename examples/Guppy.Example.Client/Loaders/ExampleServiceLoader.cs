using Autofac;
using Guppy.Attributes;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Game.Input.Enums;
using Guppy.Game.MonoGame.Primitives;
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
            services.RegisterType<CellTypeService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.RegisterGeneric(typeof(PointPrimitiveBatch<,>));
            services.RegisterGeneric(typeof(PointPrimitiveBatch<>));

            services.RegisterInput("MouseDown", CursorButtons.Right, new[]
            {
                (true, new PlaceSandInput(true)),
                (false, new PlaceSandInput(false)),
            });
        }
    }
}
