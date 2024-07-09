using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Loaders;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.MonoGame.Common.Utilities.Cameras;
using Guppy.Game.Primitives;
using Microsoft.Xna.Framework.Input;

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

            services.RegisterInput("SelectCellType_01", Keys.D1, new[]
            {
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Sand)),
            });

            services.RegisterInput("SelectCellType_02", Keys.D2, new[]
            {
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Water)),
            });

            services.RegisterInput("SelectCellType_03", Keys.D3, new[]
            {
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Plant)),
            });

            services.RegisterInput("SelectCellType_04", Keys.D4, new[]
            {
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Concrete)),
            });

            services.RegisterInput("SelectCellType_05", Keys.D5, new[]
{
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Fire)),
            });

            services.RegisterInput("SelectCellType_00", Keys.D0, new[]
            {
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Water)),
            });
        }
    }
}
