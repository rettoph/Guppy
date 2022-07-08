using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;
using SysVector4 = System.Numerics.Vector4;
using SysVector2 = System.Numerics.Vector2;
using Guppy.MonoGame.Services;
using Minnow.Collections;
using Guppy.Common;
using Guppy.MonoGame.Commands;
using Guppy.MonoGame.UI.Constants;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed partial class TerminalService : ITerminalService, ISubscriber<ToggleTerminal>
    {
        private ImGuiBatch _guiRenderer;
        private ImFontPtr _font;
        private GameWindow _window;
        private string _input;
        private Buffer<(string text, SysVector4 color)> _output;
        private bool _scrollLocked;
        private ICommandService _commands;
        private bool _focusInput;
        private bool _visible;

        public TerminalService(ImGuiBatch imGuiBatch, GameWindow window, ICommandService commands)
        {
            _guiRenderer = imGuiBatch;
            _window = window;
            // _renderer.RebuildFontAtlas();
            // _font = _renderer.BindFont(@"C:\Users\Anthony\source\repos\VoidHuntersRevived\libraries\Guppy\src\Guppy.Gaming\Content\Fonts\src\SpaceMono-Regular.ttf", 50);
            _font = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFont].Ptr;
            _input = string.Empty;
            _output = new Buffer<(string text, SysVector4 color)>(2048);
            _scrollLocked = true;
            _commands = commands;
            _focusInput = true;

            Console.SetOut(new TerminalService.TextWriter(this, null));
            Console.SetError(new TerminalService.TextWriter(this, XnaColor.Red));

            _commands.Subscribe(this);
        }

        public void WriteLine(string text, XnaColor color)
        {
            _output.Add((text, color.ToNumericsVector4()));
        }

        public void Draw(GameTime gameTime)
        {
            _guiRenderer.Begin(gameTime);

            if (_visible)
            {
                this.DrawTerminal();
            }

            _guiRenderer.End();
        }

        private void DrawTerminal()
        {
            var windowSize = new SysVector2(_window.ClientBounds.Width, _window.ClientBounds.Height);
            var inputHeight = 24;
            var outputContainerSize = new SysVector2(windowSize.X, windowSize.Y - inputHeight);
            var inputContainerSize = new SysVector2(windowSize.X, inputHeight);

            ImGui.SetNextWindowFocus();
            ImGui.SetNextWindowPos(new SysVector2(0, 0));
            ImGui.SetNextWindowSize(windowSize);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new SysVector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new SysVector2(0, 0));
            ImGui.PushFont(_font);
            ImGui.Begin("terminal", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new SysVector2(15, 15));
            ImGui.BeginChild("output-container", outputContainerSize, false, ImGuiWindowFlags.AlwaysVerticalScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);

            foreach ((string text, SysVector4 color) in _output)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, color);
                ImGui.TextWrapped(text);
                ImGui.PopStyleColor();
            }

            if (_scrollLocked != (ImGui.GetScrollY() == ImGui.GetScrollMaxY()))
            {
                _scrollLocked = !_scrollLocked;
            }

            if (_scrollLocked)
            {
                ImGui.SetScrollHereY(1);
            }

            ImGui.EndChild();
            ImGui.PopStyleVar();


            ImGui.BeginChild("input-container", inputContainerSize, false);

            ImGui.PushItemWidth(-1);
            if (_focusInput)
            {
                ImGui.SetKeyboardFocusHere(0);
                _focusInput = false;
            }

            if (ImGui.InputText("input", ref _input, 200, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                _focusInput = true;

                if(_input != string.Empty)
                {
                    _commands.Invoke(_input);
                    _input = string.Empty;
                }
            }
            ImGui.PopItemWidth();

            ImGui.EndChild();

            ImGui.End();
            ImGui.PopFont();
            ImGui.PopStyleVar();
        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public void Process(in ToggleTerminal message)
        {
            _input = string.Empty;
            _focusInput = true;
            _visible = !_visible;
        }
    }
}
