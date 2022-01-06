using Guppy.CommandLine.Services;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.IO.EventArgs;
using Guppy.IO.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serilog;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Text;

namespace Guppy.IO.Services
{
    public class TerminalService : Frameable, IConsole
    {
        #region Private Fields
        private SpriteFont _font;
        private Lazy<CommandService> _commands;
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private MouseService _mouse;
        private KeyboardService _keyboard;
        private InputCommandService _inputCommands;
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private ILogger _log;

        private IStandardStreamWriter _out;
        private IStandardStreamWriter _error;

        private TerminalStringBuilder _strings;
        private Single _stringsHeight;

        private Single _scrollPosition;
        private RasterizerState _rasterizerState;

        private Rectangle _inputBounds;
        private Rectangle _inputScissorBounds;
        private Rectangle _consoleBounds;
        private Rectangle _consoleScissorBounds;

        private Boolean _isCaretVisible;
        private ActionTimer _toggleIsCaretVisibleTimer;
        private String _input;
        private Queue<String> _inputBuffer;
        private Int32 _inputBufferOffset;
        private Int32 _inputBufferLength;
        private Int32 _maxInputBufferLength;
        #endregion

        #region Public Properties
        public SpriteFont Font
        {
            get => _font;
            set => _font = value;
        }

        public Color InputColor { get; set; }
        #endregion

        #region IConsole Implementation
        IStandardStreamWriter IStandardOut.Out => _out;

        bool IStandardOut.IsOutputRedirected => false;

        IStandardStreamWriter IStandardError.Error => _error;

        bool IStandardError.IsErrorRedirected => false;

        bool IStandardIn.IsInputRedirected => Console.IsInputRedirected;
        #endregion

        #region Lifecycle Methods
        public TerminalService()
        {

        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _font = provider.GetContent<SpriteFont>("guppy:font:debug");

            provider.ServiceLazy(out _commands);
            provider.Service(out _window);
            provider.Service(out _graphics);
            provider.Service(out _primitiveBatch);
            provider.Service(out _mouse);
            provider.Service(out _keyboard);
            provider.Service(out _inputCommands);
            provider.Service(out _log);
            provider.Service(Guppy.Constants.ServiceNames.TransientSpritebatch, out _spriteBatch);
            provider.Service(Guppy.Constants.ServiceNames.TransientCamera, out _camera);

            _strings = new TerminalStringBuilder(256);

            _out = StandardStreamWriter.Create(new CommandTerminalTextWriter(this));
            _error = StandardStreamWriter.Create(new CommandTerminalTextWriter(this));
            _toggleIsCaretVisibleTimer = new ActionTimer(750);
            _input = String.Empty;
            _inputBufferLength = 0;
            _inputBufferOffset = 0;
            _maxInputBufferLength = 25;
            _inputBuffer = new Queue<String>();
            _rasterizerState = new RasterizerState()
            {
                ScissorTestEnable = true,
                MultiSampleAntiAlias = true
            };

            _camera.Center = false;

            this.InputColor = Color.Gray;

            this.Clean();
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            // Override Console output
            Console.SetOut(new ConsoleTerminalTextWriter(this));
            Console.SetError(new ConsoleTerminalTextWriter(this));
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            this.Close();
        }
        #endregion

        #region Helper Methods
        public void Write(Char character, Color color, Guid sourceId)
        {
            _strings.Append(character, color, sourceId);
        }

        public void Write(String text, Color color, Guid sourceId)
        {
            _strings.Append(text, color, sourceId);
        }
        #endregion

        #region Frame Methods
        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _toggleIsCaretVisibleTimer.Update(gameTime, gt => _isCaretVisible = !_isCaretVisible);
            _camera.TryClean(gameTime);

            _primitiveBatch.Begin(_camera, BlendState.AlphaBlend);

            _primitiveBatch.DrawRectangle(new Color(Color.Black, 0.25f), _consoleBounds);
            _primitiveBatch.DrawRectangle(new Color(Color.LightGray, 0.25f), _inputBounds);

            if(_isCaretVisible)
                _primitiveBatch.DrawLine(
                    this.InputColor,
                    new Vector2(_font.MeasureString(_input).X + _inputScissorBounds.Left, _inputScissorBounds.Top), 
                    new Vector2(_font.MeasureString(_input).X + _inputScissorBounds.Left, _inputScissorBounds.Top + _font.LineSpacing));

            _primitiveBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate, 
                blendState: BlendState.AlphaBlend,
                rasterizerState: _rasterizerState);

            _graphics.ScissorRectangle = _consoleScissorBounds;

            _spriteBatch.DrawString(
                _font,
                _scrollPosition.ToString(),
                new Vector2(700, 100),
                Color.Cyan);

            var position = new Vector2(_consoleScissorBounds.Left, _consoleScissorBounds.Bottom);
            position.Y -= _stringsHeight;
            position.Y += _scrollPosition;
            Single stringsHeight = 0;

            foreach (TerminalString tString in _strings)
            {
                if(tString.NewLine)
                {
                    position.X = _consoleScissorBounds.Left;
                    position.Y += _font.LineSpacing;
                    stringsHeight += _font.LineSpacing;
                }

                if(tString.Text is not null)
                {
                    _spriteBatch.DrawString(
                        _font,
                        tString.Text,
                        position,
                        tString.Color);

                    position.X += _font.MeasureString(tString.Text).X;
                }
            }

            _stringsHeight = stringsHeight;
            _graphics.ScissorRectangle = _inputScissorBounds;
            _spriteBatch.DrawString(
                _font, 
                _input, 
                new Vector2(
                    _inputScissorBounds.Left, 
                    _inputScissorBounds.Top),
                this.InputColor);

            _spriteBatch.End();
        }
        #endregion

        #region Helper Methods
        public void Open(Game game = default)
        {
            if (game != default)
                game.OnDraw += this.TryDraw;

            _input = String.Empty;
            _inputBufferOffset = _inputBufferLength;

            this.Clean();

            _mouse.OnScrollWheelValueChanged += this.HandleScrollWheelValueChanged;
            _window.ClientSizeChanged += this.HandleClientSizeChanged;
            _window.TextInput += this.HandleTextInput;
            _keyboard[Keys.Up].OnState[ButtonState.Released] += this.HandleArrowReleased;
            _keyboard[Keys.Down].OnState[ButtonState.Released] += this.HandleArrowReleased;

            _inputCommands.Locked = true;
        }

        public void Close(Game game = default)
        {
            if(game != default)
                game.OnDraw -= this.TryDraw;

            _mouse.OnScrollWheelValueChanged -= this.HandleScrollWheelValueChanged;
            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
            _window.TextInput -= this.HandleTextInput;
            _keyboard[Keys.Up].OnState[ButtonState.Released] -= this.HandleArrowReleased;
            _keyboard[Keys.Down].OnState[ButtonState.Released] -= this.HandleArrowReleased;

            _inputCommands.Locked = false;
        }

        private void Clean()
        {
            _scrollPosition = 0;
            _inputBounds = new Rectangle()
            {
                X = 0,
                Y = _graphics.Viewport.Bounds.Height - _font.LineSpacing - 10,
                Width = _graphics.Viewport.Bounds.Width,
                Height = _font.LineSpacing + 10,
            };

            _inputScissorBounds = new Rectangle()
            {
                X = 7,
                Y = _graphics.Viewport.Bounds.Height - _font.LineSpacing - 5,
                Width = _graphics.Viewport.Bounds.Width - 14,
                Height = _font.LineSpacing,
            };

            _consoleBounds = new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = _graphics.Viewport.Width,
                Height = _graphics.Viewport.Height - _inputBounds.Height
            };

            _consoleScissorBounds = new Rectangle()
            {
                X = 10,
                Y = 10,
                Width = _consoleBounds.Width - 20,
                Height = _consoleBounds.Height - 20
            };
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

        #region Events
        private void HandleScrollWheelValueChanged(MouseService sender, ScrollWheelArgs args)
        {
            _scrollPosition += (args.Delta / 120) * _font.LineSpacing;
            _scrollPosition = MathHelper.Clamp(_scrollPosition, 0, _stringsHeight - _consoleBounds.Height);
        }

        private void HandleClientSizeChanged(object sender, System.EventArgs e)
            => this.Clean();

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            switch(e.Key)
            {
                case Keys.Back:
                    if(_input.Length > 0)
                        _input = _input.Remove(_input.Length - 1, 1);
                    break;
                case Keys.Enter:
                    if (_input == String.Empty)
                        return;

                    this.Write($"> {_input}", this.InputColor, this.Id);
                    _inputBuffer.Enqueue(_input);

                    if (_inputBufferLength < _maxInputBufferLength)
                    {
                        _inputBufferLength++;
                    }
                    else
                    {
                        _inputBuffer.Dequeue();
                    }

                    try
                    {
                        _commands.Value.Invoke(_input);
                    }
                    catch(Exception err)
                    {
                        _log.Error(err, err.Message);
                    }
                    finally
                    {
                        _input = String.Empty;
                        _inputBufferOffset = _inputBufferLength;
                    }
                    break;
                default:
                    if (_font.Characters.Contains(e.Character))
                        _input += e.Character;
                    break;
            }
        }

        private void HandleArrowReleased(InputButtonManager sender, InputButtonArgs args)
        {
            switch (args.Which.KeyboardKey)
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
