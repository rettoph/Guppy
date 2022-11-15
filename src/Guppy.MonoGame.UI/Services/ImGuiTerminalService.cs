using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;
using NumVector4 = System.Numerics.Vector4;
using NumVector2 = System.Numerics.Vector2;
using Guppy.MonoGame.Services;
using Guppy.Common;
using Guppy.MonoGame.UI.Constants;
using Guppy.Common.Collections;
using Guppy.MonoGame.Messages.Inputs;
using Guppy.Attributes;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed partial class ImGuiTerminalService : BaseWindowService, ITerminalService
    {
        private readonly static Buffer<(string text, NumVector4 color)> _output;
        private readonly static TextWriter _outWriter;
        private readonly static TextWriter _errWriter;

        private readonly ImGuiBatch _imGuiBatch;
        private readonly ImFontPtr _font;
        private readonly GameWindow _window;
        
        private readonly Lazy<ICommandService> _commands;
        private bool _focusInput;
        private string _input;
        private bool _scrollLocked;



        public override ToggleWindowInput.Windows Window => ToggleWindowInput.Windows.Terminal;

        public ImGuiTerminalService(ImGuiBatch imGuiBatch, GameWindow window, Lazy<ICommandService> commands) : base(false)
        {
            _imGuiBatch = imGuiBatch;
            _window = window;
            _font = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFont].Ptr;
            _input = string.Empty;
            _scrollLocked = true;
            _commands = commands;
            _focusInput = true;
        }

        static ImGuiTerminalService()
        {
            _output = new Buffer<(string text, NumVector4 color)>(2048);

            _outWriter = new ImGuiTerminalService.TextWriter(null);
            _errWriter = new ImGuiTerminalService.TextWriter(XnaColor.Red);

            Console.SetOut(_outWriter);
            Console.SetError(_errWriter);
        }

        void ITerminalService.WriteLine(string text, XnaColor color)
        {
            ImGuiTerminalService.WriteLine(text, color);
        }

        internal static void WriteLine(string text, XnaColor color)
        {
            _output.Add((text, color.ToNumericsVector4()));
        }

        public override void Draw(GameTime gameTime)
        {
            _imGuiBatch.Begin(gameTime);

            var windowSize = new NumVector2(_window.ClientBounds.Width, _window.ClientBounds.Height);
            var inputHeight = 24;
            var outputContainerSize = new NumVector2(windowSize.X, windowSize.Y - inputHeight);
            var inputContainerSize = new NumVector2(windowSize.X, inputHeight);

            ImGui.SetNextWindowFocus();
            ImGui.SetNextWindowPos(new NumVector2(0, 0));
            ImGui.SetNextWindowSize(windowSize);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new NumVector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new NumVector2(0, 0));
            ImGui.PushFont(_font);
            ImGui.Begin("terminal", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new NumVector2(15, 15));
            ImGui.BeginChild("output-container", outputContainerSize, false, ImGuiWindowFlags.AlwaysVerticalScrollbar | ImGuiWindowFlags.AlwaysUseWindowPadding);

            foreach ((string text, NumVector4 color) in _output)
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
                    _commands.Value.Invoke(_input);
                    _input = string.Empty;
                }
            }
            ImGui.PopItemWidth();

            ImGui.EndChild();

            ImGui.End();
            ImGui.PopFont();
            ImGui.PopStyleVar(2);

            _imGuiBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
