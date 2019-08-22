using Guppy;
using Guppy.Extensions;
using Guppy.Utilities.Loggers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GuppyLoader guppy;
        PongGame pong;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.guppy = new GuppyLoader();
        }

        protected override void Initialize()
        {
            base.Initialize();

            guppy.ConfigureLogger<ConsoleLogger>()
                .ConfigureMonoGame(this.graphics, this.Content, this.Window)
                .Initialize();

            this.pong = guppy.BuildGame<PongGame>();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.pong.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.pong.TryUpdate(gameTime);
        }
    }
}
