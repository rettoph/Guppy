using Guppy.CommandLine.Services;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.Extensions.Utilities;
using Guppy.IO.Structs;
using Guppy.IO.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private CommandService _commands;
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private MouseService _mouse;
        private KeyboardService _keyboard;
        private InputCommandService _inputCommands;
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;

        private Color _outColor = Color.White;
        private Color _errorColor = Color.Red;
        private IStandardStreamWriter _out;
        private IStandardStreamWriter _error;

        private TerminalString[] _lines;
        private Int32 _lineCount;
        private Int32 _lineIndex;

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
        public Color OutColor
        {
            get => _outColor;
            set => _outColor = value;
        }

        public Color ErrorColor
        {
            get => _errorColor;
            set => _errorColor = value;
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
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            // Override Console output
            Console.SetOut(new ConsoleTerminalTextWriter(this));
            Console.SetError(new ConsoleTerminalTextWriter(this));

            _font = provider.GetContent<SpriteFont>("guppy:font:debug");

            provider.Service(out _commands);
            provider.Service(out _window);
            provider.Service(out _graphics);
            provider.Service(out _primitiveBatch);
            provider.Service(out _mouse);
            provider.Service(out _keyboard);
            provider.Service(out _inputCommands);
            provider.Service(Guppy.Constants.ServiceConfigurationKeys.TransientSpritebatch, out _spriteBatch);
            provider.Service(Guppy.Constants.ServiceConfigurationKeys.TransientCamera, out _camera);

            _lines = new TerminalString[256];
            _lineIndex = -1;
            _out = StandardStreamWriter.Create(new CommandTerminalTextWriter(this, ref _outColor));
            _error = StandardStreamWriter.Create(new CommandTerminalTextWriter(this, ref _errorColor));
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

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Close();

            _commands = default;
            _window = default;
            _graphics = default;
            _primitiveBatch = default;
            _mouse = default;
            _keyboard = default;
            _spriteBatch = default;
            _camera = default;
        }
        #endregion

        #region Helper Methods
        public void Write(String text, Color color)
        {
            foreach(String line in text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                _lines[(_lineIndex = (_lineIndex + 1) % _lines.Length)] = new TerminalString()
                {
                    Color = color,
                    Text = new String(line.Where(c => _font.Characters.Contains(c)).ToArray())
                };

                _lineCount = Math.Min(_lineCount + 1, _lines.Length);
            }
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

            var position = new Vector2(_consoleScissorBounds.Left, _consoleScissorBounds.Bottom);
            position.Y -= _font.LineSpacing;
            position.Y += _scrollPosition;

            for (var i=0; i<_lineCount; i++)
            {
                var index = (_lineIndex - i);
                if (index < 0)
                    index = _lines.Length + index;

                var tString = _lines[index];
                _spriteBatch.DrawString(_font, tString.Text, position, tString.Color);

                position.Y -= _font.LineSpacing;
            }

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
            _scrollPosition = MathHelper.Clamp(_scrollPosition, 0, (_lineCount * _font.LineSpacing) - _consoleBounds.Height);
        }

        private void HandleClientSizeChanged(object sender, EventArgs e)
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

                    this.Write($"> {_input}", this.InputColor);
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
                        _commands.Invoke(_input);
                    }
                    catch(Exception err)
                    {
                        this.log.Error(err.Message);
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
