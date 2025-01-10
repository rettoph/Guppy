using Guppy.Engine;
using Guppy.Game.Common;
using Guppy.Game.MonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame
{
    public class GuppyMonoGame : Microsoft.Xna.Framework.Game
    {
        private readonly GuppyContext _context;
        private readonly GraphicsDeviceManager _graphics;
        private GameEngine? _engine;


        // https://community.monogame.net/t/start-in-maximized-window/12264
        // [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        // public static extern void SDL_MaximizeWindow(IntPtr window);

        public GuppyMonoGame(GuppyContext context)
        {
            this._graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.IsFixedTimeStep = false;

            this._graphics.PreparingDeviceSettings += (s, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            this._graphics.SynchronizeWithVerticalRetrace = false;
            this._graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this._graphics.ApplyChanges();


            this._context = context;
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
                var engine = new GameEngine(this._context, builder =>
                {
                    builder.RegisterMonoGameServices(this, this._graphics, this.Content, this.Window);
                });

                this.Initialize(engine);

                this._engine = engine;
            });
        }

        protected virtual void Initialize(IGameEngine engine)
        {

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
        protected override void UnloadContent() =>
            // TODO: Unload any non ContentManager content here
            this._engine?.Dispose();

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

            throw new NotImplementedException();
            // _engine?.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.GraphicsDevice.Clear(Color.Black);

            throw new NotImplementedException();
            // _engine?.Draw(gameTime);
        }
    }

    public class GuppyMonoGame<TGuppy>(GuppyContext context) : GuppyMonoGame(context)
        where TGuppy : class, IScene
    {
        protected override void Initialize(IGameEngine engine)
        {
            base.Initialize(engine);

            engine.Scenes.Create<TGuppy>();
        }
    }
}