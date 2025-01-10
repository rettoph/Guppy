using Autofac;
using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Example.Client.Utilities;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Input.Common.Enums;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Example.Client.Modules
{
    public class ExampleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CellTypeService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AirCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<AshCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<ConcreteCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<FireCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<PlantCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<SandCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<SmolderCellType>().As<ICellType>().InstancePerLifetimeScope();
            builder.RegisterType<WaterCellType>().As<ICellType>().InstancePerLifetimeScope();

            builder.RegisterType<World>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterSceneFilter<World, MainScene>();

            builder.RegisterGeneric(typeof(StaticPrimitiveBatch<,>));
            builder.RegisterGeneric(typeof(StaticPrimitiveBatch<>));

            builder.RegisterGeneric(typeof(PointPrimitiveBatch<,>));
            builder.RegisterGeneric(typeof(PointPrimitiveBatch<>));

            builder.RegisterInput("MouseDown", CursorButtonsEnum.Right,
            [
                (true, new PlaceSandInput(true)),
                (false, new PlaceSandInput(false)),
            ]);

            builder.RegisterInput("SelectCellType_01", Keys.D1,
            [
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Sand)),
            ]);

            builder.RegisterInput("SelectCellType_02", Keys.D2,
            [
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Water)),
            ]);

            builder.RegisterInput("SelectCellType_03", Keys.D3,
            [
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Plant)),
            ]);

            builder.RegisterInput("SelectCellType_04", Keys.D4,
            [
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Concrete)),
            ]);

            builder.RegisterInput("SelectCellType_05", Keys.D5,
[
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Fire)),
            ]);

            builder.RegisterInput("SelectCellType_00", Keys.D0,
            [
                (true, new SelectCellTypeInput(Enums.CellTypeEnum.Water)),
            ]);
        }
    }
}