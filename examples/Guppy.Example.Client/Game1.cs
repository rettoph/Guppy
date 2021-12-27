using Guppy.EntityComponent.Interfaces;
using Guppy.Example.Library;
using Guppy.Extensions;
using Guppy.Extensions.Utilities;
using Guppy.IO.Extensions;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Client
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PrimitiveBatch<VertexPositionColor> primitiveBatch;
        Matrix projection;
        
        Queue<float> _frameTimes = new Queue<float>();
        int _frameCount = 0;
        int _maxFrameCount = 800;

        float _maxFrameTime = float.MinValue;

        GuppyLoader guppy;
        ExampleGame game;

#if WINDOWS
        // https://community.monogame.net/t/start-in-maximized-window/12264
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MaximizeWindow(IntPtr window);
#endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            guppy = new GuppyLoader();

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.IsFixedTimeStep = false;

            this.graphics.PreparingDeviceSettings += (s, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.graphics.ApplyChanges();

            primitiveBatch = new PrimitiveBatch<VertexPositionColor>(this.GraphicsDevice);

            this.projection = projection = Matrix.CreateOrthographicOffCenter(
                    0,
                    800,
                    480,
                    0,
                    0f,
                    1f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            this.game = guppy
                .ConfigureMonoGame(this.graphics, this.Content, this.Window)
                .ConfigureTerminal()
                .Initialize()
                .BuildGame<ExampleGame>();
#if WINDOWS
            SDL_MaximizeWindow(Window.Handle);
#endif
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // game.TryDispose();
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

            game.TryUpdate(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            game.TryDraw(gameTime);
            return;
            this.GraphicsDevice.Clear(Color.Black);

            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameTimes.Enqueue(totalSeconds);

            if(_frameCount++ > _maxFrameCount)
            {
                _frameTimes.Dequeue();
                _frameCount--;
            }

            _maxFrameTime *= 0.99999f;
            if(totalSeconds > _maxFrameTime)
            {
                _maxFrameTime = totalSeconds;
            }



            primitiveBatch.Begin(view: Matrix.Identity, projection: this.projection, world: Matrix.Identity);

            float y;
            float x = 0;

            foreach (float frameTime in _frameTimes)
            {
                y = (frameTime / _maxFrameTime) * 480;

                primitiveBatch.DrawLine(
                    Color.Red, x, 480, 0, 
                    Color.Red, x, 480 - y, 0);

                x++;
            }

            primitiveBatch.End();
        }
    }
}
