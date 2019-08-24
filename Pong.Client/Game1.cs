using Guppy;
using Guppy.Extensions;
using Guppy.Factories;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pong.Client
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GuppyLoader guppy;
        Guppy.Game game;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.guppy = new GuppyLoader();

           //this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;

            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 900;
        }

        protected override void Initialize()
        {
            base.Initialize();

            new Thread(new ThreadStart(() =>
            {
                guppy.ConfigureLogger<ConsoleLogger>().ConfigureMonoGame(this.graphics, this.Content, this.Window).Initialize();
                this.game = guppy.BuildGame<PongGame>();
            })).Start();
            
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.game?.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.game?.TryUpdate(gameTime);
        }
    }
}
