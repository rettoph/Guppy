using Autofac;
using Guppy.Core.Commands.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Logging.Common.Extensions;
using Guppy.Core.Logging.Serilog.Extensions;
using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Example.Client.Utilities;
using Guppy.Game;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.MonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Example.Client
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameEngine? _engine;


        // https://community.monogame.net/t/start-in-maximized-window/12264
        // [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        // public static extern void SDL_MaximizeWindow(IntPtr window);


        public Game1()
        {
            this._graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.IsFixedTimeStep = false;

            this._graphics.PreparingDeviceSettings += (s, e) =>
            {
                this._graphics.PreferMultiSampling = true;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
                e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            this._graphics.SynchronizeWithVerticalRetrace = false;
            this._graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this._graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // SDL_MaximizeWindow(this.Window.Handle);
            Task.Run(() =>
            {
                GameEngine engine = new GameEngine(EnvironmentVariablesBuilder.Build("rettoph", "example"), builder =>
                {
                    builder.RegisterMonoGameServices(this, this._graphics, this.Content, this.Window)
                        .RegisterSerilogLoggingServices();

                    builder.ConfigureConsoleLogMessageSink(enabled: true);

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
                }).Start();

                engine.Scenes.Create<MainScene>();

                this._engine = engine;
            });

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this._engine?.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this._engine?.Dispose();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            this._engine?.Dispose();

            Environment.Exit(0);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);

            this._engine?.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this._engine is null)
            {
                return;
            }

            this.GraphicsDevice.Clear(Color.Black);

            this._engine?.Draw(gameTime);
        }
    }
}