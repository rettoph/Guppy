using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demos.Pong
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private GuppyLoader _guppy;
        private PongGame _game;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _guppy = new GuppyLoader();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _guppy.Services.ConfigureMonoGame(this.Window, _graphics);
            _game = _guppy.Initialize().BuildGame<PongGame>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _game.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.TryDraw(gameTime);
        }
    }
}
