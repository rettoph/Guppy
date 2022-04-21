using Guppy.EntityComponent;
using Guppy.Gaming.Constants;
using Guppy.Gaming.Enums;
using Guppy.Gaming.Graphics;
using Guppy.Gaming.Graphics.PrimitiveShapes;
using Guppy.Gaming.Messages;
using Guppy.Gaming.Providers;
using Guppy.Providers;
using Guppy.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minnow.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class TerminalService : ITerminalService, ISubscriber<TerminalActionMessage>
    {
        private SpriteBatch _spriteBatch;
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private Camera2D _camera;
        private ISettingProvider _settings;
        private ICommandService _commands;
        private IColorProvider _colors;
        private IContentProvider _content;
        private bool _active;
        private Buffer<string> _lines;
        private Buffer<string> _history;
        private int _historyPos;
        private GameWindow _window;
        private int _inputHeight;
        private Color _backgroundTextColor;
        private Color _inputTextColor;
        private RectangleShape _inputBackground;
        private RectangleShape _background;
        private Content<SpriteFont> _font;
        private string _input;

        public TerminalService(
            SpriteBatch spriteBatch,
            PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Camera2D camera,
            GameWindow window,
            ICommandService commands,
            ISettingProvider settings,
            IColorProvider colors,
            IContentProvider content)
        {
            _spriteBatch = spriteBatch;
            _primitiveBatch = primitiveBatch;
            _camera = camera;
            _window = window;
            _commands = commands;
            _colors = colors;
            _content = content;
            _settings = settings;
            _lines = new Buffer<string>(_settings.Get<int>(SettingConstants.TerminalBufferLength).Value);
            _history = new Buffer<string>(_settings.Get<int>(SettingConstants.TerminalBufferLength).Value);
            _historyPos = 0;
            _active = false;
            _input = string.Empty;

            _camera.Center = false;

            _font = _content.Get<SpriteFont>(ContentConstants.DiagnosticsFont);
            _inputHeight = (int)_font.Value.LineSpacing + 6;

            _inputTextColor = _colors[ColorConstants.TerminalInputTextColor];
            _backgroundTextColor = _colors[ColorConstants.TerminalBackgroundTextColor];

            _inputBackground = new RectangleShape(
                position: new Vector2(0, _window.ClientBounds.Height - _inputHeight),
                rotation: 0,
                width: _window.ClientBounds.Width,
                height: _inputHeight,
                color: _colors[ColorConstants.TerminalInputBackgroundColor]);
            _background = new RectangleShape(
                position: new Vector2(0, 0),
                rotation: 0,
                width: _window.ClientBounds.Width,
                height: _window.ClientBounds.Height,
                color: _colors[ColorConstants.TerminalBackgroundColor]);

            _commands.Subscribe(this);
            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        public void Dispose()
        {
            _commands.Unsubscribe(this);
            _window.ClientSizeChanged -= this.HandleClientSizeChanged;
        }

        public bool Process(in TerminalActionMessage message)
        {
            switch (message.Action)
            {
                case TerminalAction.Toggle:
                    _active = !_active;

                    if (_active)
                    {
                        _window.TextInput += this.HandleTextInput;
                        _input = string.Empty;
                    }

                    if (!_active)
                    {
                        _window.TextInput -= this.HandleTextInput;
                    }
                    break;
                case TerminalAction.Prev:
                    _historyPos = (_historyPos == 0 ? _history.Size : _historyPos) - 1;
                    _input = _history.ElementAt(_historyPos % _history.Size);
                    break;
                case TerminalAction.Next:
                    _historyPos = (_historyPos + 1) % _history.Size;
                    _input = _history.ElementAt(_historyPos);
                    break;
            }

            return true;
        }

        void ITerminalService.Draw(GameTime gameTime)
        {
            if(!_active)
            {
                return;
            }

            _camera.TryClean(gameTime);

            // Draw background...
            _primitiveBatch.Begin(_camera);

            _primitiveBatch.Fill(_background);
            _primitiveBatch.Fill(_inputBackground);

            _primitiveBatch.End();

            // Draw text...
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_font, _input, Vector2.One * 20, _backgroundTextColor);

            _spriteBatch.End();
        }

        void ITerminalService.Update(GameTime gameTime)
        {
            if(_active)
            {
                return;
            }
        }

        private void HandleTextInput(object? sender, TextInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Back:
                    if(_input.Length == 0)
                    {
                        return;
                    }

                    _input = _input.Remove(_input.Length - 1, 1);
                    break;
                case Keys.Enter:
                    _commands.Invoke(_input);
                    _history.Add(_input);
                    _historyPos++;
                    _input = string.Empty;
                    break;
                default:
                    if(_font.Value.Characters.Contains(e.Character))
                    {
                        _input += e.Character;
                    }
                    break;
            }
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            _inputBackground.Width = _window.ClientBounds.Width;
            _inputBackground.Position = new Vector2(0, _window.ClientBounds.Height - _inputHeight);

            _background.Width = _window.ClientBounds.Width;
            _background.Height = _window.ClientBounds.Height;
        }
    }
}
