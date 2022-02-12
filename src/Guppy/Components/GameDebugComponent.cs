using Guppy.CommandLine.Services;
using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Messages;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Components
{
    internal class GameDebugComponent : Component<Game>,
        IDataProcessor<DebugFpsCommand>
    {
        #region Private Fields
        private Double[] _frameBuffer;
        private Int32 _frameBufferIndex;

        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private Camera2D _camera;
        private Queue<Single> _groupTimes;
        private Int32 _groupCount;
        private Single _maxGroupTime = Single.MinValue;

        private Double _groupValue;
        private Int32 _framesPerGroup = 100;
        private Int32 _framesCount;

        private Int32 _width = 400;
        private Int32 _height = 200;

        private CommandService _commands;

        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _commands);
            provider.Service(out _primitiveBatch);
            provider.Service(out _spriteBatch);
            provider.BuildService(out _camera);

            _font = provider.GetContent<SpriteFont>(Constants.Content.DebugFont);

            _camera.Center = false;

            this.Entity.OnPostDraw += this.Draw;
            this.Entity.OnUpdate += this.Update;

            _groupTimes = new Queue<float>();
            _frameBuffer = new Double[1000];

            _commands.RegisterProcessor<DebugFpsCommand>(this);
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            this.Entity.OnPostDraw -= this.Draw;
            this.Entity.OnUpdate -= this.Update;
        }
        #endregion

        #region Frame Methods
        private void Update(GameTime gameTime)
        {
            _frameBuffer[_frameBufferIndex++ % _frameBuffer.Length] = gameTime.ElapsedGameTime.TotalMilliseconds;

            _groupValue += gameTime.ElapsedGameTime.TotalMilliseconds;
            _framesCount++;
            

            if (_framesCount >= _framesPerGroup)
            {
                _groupTimes.Enqueue((Single)_groupValue);

                if (_groupCount++ >= _width)
                {
                    _groupTimes.Dequeue();
                    _groupCount--;
                }

                // _maxGroupTime *= (Single)Math.Pow(0.9999f, _framesPerGroup);

                if (_groupValue > _maxGroupTime)
                {
                    _maxGroupTime = (Single)_groupValue;
                }

                _groupValue = 0;
                _framesCount = 0;
            }
        }

        private void Draw(GameTime gameTime)
        {
            _camera.TryClean(gameTime);

            _primitiveBatch.Begin(_camera);

            _primitiveBatch.DrawRectangle(new Color(Color.Black, 0.1f), new Rectangle(0, 0, _width, _height));
            _primitiveBatch.TryFlushTriangleVertices(true);

            Single oldY = 0;
            Single y;
            Single x = _width - _groupCount;

            foreach(Single frameTime in _groupTimes)
            {
                y = (frameTime / _maxGroupTime) * _height;

                _primitiveBatch.DrawLine(Color.Red, x, _height - oldY, 0, Color.Red, x, _height - y, 0);

                x++;

                oldY = y;
            }

            _primitiveBatch.DrawRectangle(new Color(Color.Black, 0.3f), new Rectangle(0, 0, _width, _height));
            _primitiveBatch.TryFlushLineVertices(true);

            _primitiveBatch.End();

            StringBuilder sb = new StringBuilder();
            sb.Append("Frames Per Group: ")
                .AppendLine(_framesPerGroup.ToString())
                .Append("Max Group Time: ")
                .Append(_maxGroupTime.ToString("#,000.00"))
                .Append("ms (")
                .Append((1000f / (_maxGroupTime / _framesPerGroup)).ToString("#,##0.0"))
                .AppendLine("fps)\n")
                .Append("FPS: ")
                .Append((1000 / _frameBuffer.Average()).ToString("#,##0.0"));

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, sb, Vector2.One * 15, Color.WhiteSmoke);
            _spriteBatch.End();
        }
        #endregion

        #region Data Processors
        Boolean IDataProcessor<DebugFpsCommand>.Process(DebugFpsCommand message)
        {
            if(message.ResetFps)
            {
                _maxGroupTime = Single.MinValue;
            }

            return true;
        }
        #endregion
    }
}
