using Guppy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Scenes
{
    /// <summary>
    /// Special scene designed for transitioning
    /// between two different scenes in a fade to black
    /// </summary>
    public class TransitionScene : Scene
    {
        private Scene _scene1;
        private Scene _scene2;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private GameWindow _window;

        private Single _alpha;
        private Boolean _return;

        private Texture2D _overlay;

        public TransitionScene(GameWindow window, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, IServiceProvider provider) : base(provider)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _window = window;
            _overlay = new Texture2D(_graphicsDevice, 1, 1);
            _overlay.SetData<Color>(new Color[] { Color.White });
        }

        public void Set(Scene scene1, Scene scene2)
        {
            if (scene1 == this || scene2 == this)
                throw new Exception("Unable to transition into myself!");

            _scene1 = scene1;
            _scene2 = scene2;

            this.SetEnabled(true);
            this.SetVisible(true);

            _scene1.SetVisible(false);
            _scene1.SetEnabled(false);
            _scene2.SetVisible(false);
            _scene2.SetEnabled(false);

            _alpha = 0;
            _return = false;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(_return)
            {
                _scene2.Draw(gameTime);
            }
            else
            {
                _scene1.Draw(gameTime);
            }

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _spriteBatch.Draw(_overlay, new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height), Color.Lerp(Color.Transparent, Color.Black, _alpha));
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_return)
            {
                _scene2.Update(gameTime);

                _alpha = MathHelper.Lerp(_alpha, 0, (Single)gameTime.ElapsedGameTime.TotalMilliseconds / 200);

                if (_alpha <= 0.05)
                {
                    _alpha = 0;
                    _return = false;

                    this.SetEnabled(false);
                    this.SetVisible(false);

                    _scene2.SetEnabled(true);
                    _scene2.SetVisible(true);
                }
            }
            else
            {
                _scene1.Update(gameTime);

                _alpha = MathHelper.Lerp(_alpha, 1, (Single)gameTime.ElapsedGameTime.TotalMilliseconds / 200);

                if(_alpha >= 0.95)
                {
                    _alpha = 1;
                    _return = true;

                    _scene2.Update(gameTime);
                }
            }
        }
    }
}
