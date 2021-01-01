using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.IO.Commands.Services;
using Guppy.IO.Input;
using Guppy.IO.Input.Services;
using Guppy.Utilities;
using log4net.Appender;
using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Output.Services
{
    public sealed class TerminalService : Frameable, IAppender
    {
        #region Static Fields
        public static Int32 MaxMessages { get; set; } = 250;
        public static Int32 MaxInputBuffer { get; set; } = 25;
        #endregion

        #region Private Fields
        private Queue<(Color color, String text)> _messages;
        private Int32 _messageCount;

        private Synchronizer _synchronizer;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphics;
        private GameWindow _window;
        private CommandService _commands;
        private InputCommandService _inputCommands;
        private KeyboardService _keyboard;

        private Texture2D _background;
        private SpriteFont _font;

        private Vector2 _size;

        private Boolean _renderCarret;

        private String _input;
        private ActionTimer _carretActionTimer;

        private Queue<String> _inputBuffer;
        private Int32 _inputBufferOffset;
        public Int32 _inputBufferLength;
        #endregion

        #region Public Properties
        public String Name { get; set; }

        /// <summary>
        /// The <see cref="Color"/> to render a <see cref="LoggingEvent"/>
        /// based on the <see cref="LoggingEvent.Level"/> value.
        /// 
        /// Defaults are defined, but may be overwritten.
        /// </summary>
        public Dictionary<Level, Color> LevelColor { get; private set; }

        /// <summary>
        /// The color input text should be.
        /// </summary>
        public Color InputColor { get; set; }

        /// <summary>
        /// The background color overlay.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// The color to make the input bar background.
        /// </summary>
        public Color InputBackgroundColor { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _inputBuffer = new Queue<String>();
            _inputBufferLength = 0;
            _inputBufferOffset = 0;
            _messageCount = 0;
            _messages = new Queue<(Color color, String text)>();
            _carretActionTimer = new ActionTimer(750);

            this.InputColor = Color.White;
            this.InputBackgroundColor = new Color(Color.Gray, 0.5f);
            this.BackgroundColor = new Color(Color.Black, 0.5f);

            this.LevelColor = new Dictionary<Level, Color>();
            this.LevelColor[Level.Verbose] = Color.Cyan;
            this.LevelColor[Level.Debug] = Color.Magenta;
            this.LevelColor[Level.Info] = Color.White;
            this.LevelColor[Level.Warn] = Color.Yellow;
            this.LevelColor[Level.Error] = Color.Red;
            this.LevelColor[Level.Critical] = Color.DarkRed;

            provider.Service(out _synchronizer);
            provider.Service(out _spriteBatch);
            provider.Service(out _graphics);
            provider.Service(out _window);
            provider.Service(out _commands);
            provider.Service(out _inputCommands);
            provider.Service(out _keyboard);

            _font = provider.GetContent<SpriteFont>("font:terminal");
            _background = _graphics.BuildPixel(Color.White);
        }

        protected override void Release()
        {
            base.Release();

            this.Close();

            _inputBuffer.Clear();
            _messages.Clear();
            _background.Dispose();

            _spriteBatch = null;
            _graphics = null;
            _window = null;
            _commands = null;
            _inputCommands = null;
            _keyboard = null;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Toggle _renderCarret at our desired interval.
            _carretActionTimer.Update(gameTime, gt => _renderCarret = !_renderCarret);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _spriteBatch.Draw(_background, _graphics.Viewport.Bounds, this.BackgroundColor);

            Vector2 position = Vector2.Zero;
            position.X = 15;
            position.Y -= _size.Y;
            position.Y += _graphics.Viewport.Height;
            position.Y -= 15;
            position.Y -= 30;

            foreach(var message in _messages)
            {
                _spriteBatch.DrawString(_font, message.text, position, message.color);

                if (message.text.StartsWith(Environment.NewLine))
                {
                    position.X = 15;
                    position.Y += Math.Max(_font.LineSpacing, _font.MeasureString(message.text).Y - _font.LineSpacing);
                }
                else
                {
                    position.X += _font.MeasureString(message.text).X;
                }
            }

            var inputPosition = new Rectangle(0, _graphics.Viewport.Height - 30, _graphics.Viewport.Width, 30);
            _spriteBatch.Draw(_background, inputPosition, this.InputBackgroundColor);

            var inputSize = _font.MeasureString(_input ?? "");

            if (_renderCarret)
                _spriteBatch.Draw(_background, new Rectangle((Int32)inputSize.X + 15, inputPosition.Top + ((inputPosition.Height - _font.LineSpacing) / 2), 1, _font.LineSpacing), this.InputColor);

            _spriteBatch.DrawString(_font, _input ?? "", new Vector2(15f, inputPosition.Top + ((inputPosition.Height - _font.LineSpacing) / 2)), this.InputColor);

            _spriteBatch.End();
        }
        #endregion

        #region Helper Methods
        public void Open()
        {
            _inputCommands.Locked = true;
            _input = "";
            _window.TextInput += this.HandleTextInput;
            _keyboard[Keys.Up].OnState[ButtonState.Released] += this.HandleArrowReleased;
            _keyboard[Keys.Down].OnState[ButtonState.Released] += this.HandleArrowReleased;
        }

        public void Close()
        {
            _inputCommands.Locked = false;
            _window.TextInput -= this.HandleTextInput;
            _keyboard[Keys.Up].OnState[ButtonState.Released] -= this.HandleArrowReleased;
            _keyboard[Keys.Down].OnState[ButtonState.Released] -= this.HandleArrowReleased;
        }

        public void AddMessage(Color color, String text)
        {
            _synchronizer.Enqueue(gt =>
            {
                _messages.Enqueue((color, text));

                // Ensure that only N number of messages are rendered per frame.
                if (_messageCount < TerminalService.MaxMessages)
                    _messageCount++;
                else
                    _messages.Dequeue();

                // Measure the current size of the full log output...
                _size = _font.MeasureString(_messages.Select(m => m.text).Aggregate((t1, t2) => t1 + t2));
            });
        }

        private void ChangeInputBuffer(Int32 delta)
        {
            _inputBufferOffset = (_inputBufferOffset + delta) % (_inputBufferLength + 1);
            if (_inputBufferOffset < 0)
                _inputBufferOffset = _inputBufferLength;

            if (_inputBufferOffset == _inputBufferLength)
                _input = "";
            else
                _input = _inputBuffer.ElementAt(_inputBufferOffset);
        }
        #endregion

        #region IAppender Implementation
        void IAppender.DoAppend(LoggingEvent loggingEvent)
        {
            Color color;

            if (this.LevelColor.TryGetValue(loggingEvent.Level, out color))
                this.AddMessage(color, Environment.NewLine + loggingEvent.RenderedMessage);
            else
                this.AddMessage(Color.White, Environment.NewLine + loggingEvent.RenderedMessage);
        }

        void IAppender.Close()
            => this.TryRelease();
        #endregion

        #region Event Handlers
        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Back:
                    if (_input.Length > 0)
                        _input = _input.Substring(0, _input.Length - 1);
                    break;
                case Keys.Enter:
                    if (_input == "")
                        return; // Do nothing with no input

                    this.AddMessage(Color.White, Environment.NewLine + _input);
                    _commands.TryExecute(_input);

                    _inputBuffer.Enqueue(_input);

                    if (_inputBufferLength < TerminalService.MaxInputBuffer)
                    {
                        _inputBufferLength++;
                    }
                    else
                    {
                        _inputBuffer.Dequeue();
                    }

                    _input = "";
                    _inputBufferOffset = _inputBufferLength;
                    break;
                default:
                    if (_font.Characters.Contains(e.Character))
                        _input += e.Character;
                    break;
            }
        }

        private void HandleArrowReleased(InputManager sender, InputArgs args)
        {
            switch(args.Which.KeyboardKey)
            {
                case Keys.Up:
                    this.ChangeInputBuffer(-1);
                    break;
                case Keys.Down:
                    this.ChangeInputBuffer(1);
                    break;
            }
        }
        #endregion
    }
}
