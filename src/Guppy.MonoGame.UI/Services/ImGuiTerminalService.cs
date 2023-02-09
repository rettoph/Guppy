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
using Guppy.Attributes;
using Microsoft.Extensions.Options;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.UI.Providers;
using MonoGame.Extended;
using Guppy.MonoGame.Messages;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed partial class ImGuiTerminalService : SimpleDrawableGameComponent, 
        ITerminalService,
        ISubscriber<Toggle<ITerminalService>>
    {
        private readonly static Buffer<(string text, NumVector4 color)> _output;
        private readonly static TextWriter _outWriter;
        private readonly static TextWriter _errWriter;

        private readonly ImGuiBatch _imGuiBatch;
        private ImFontPtr _font;
        private readonly GameWindow _window;
        
        private readonly Lazy<ICommandService> _commands;
        private bool _focusInput;
        private string _input;
        private bool _scrollLocked;

        public ImGuiTerminalService(
            IImGuiBatchProvider batches,
            GameWindow window, 
            Lazy<ICommandService> commands)
        {
            _imGuiBatch = batches.Get(ImGuiBatchConstants.Debug);
            _window = window;
            _font = default!;
            _input = string.Empty;
            _scrollLocked = true;
            _commands = commands;
            _focusInput = true;

            this.IsEnabled = false;
            this.Visible = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            _font = _imGuiBatch.Fonts[ResourceConstants.DiagnosticsImGuiFont].Ptr;
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

            if (ImGui.InputText("terminal-input", ref _input, 200, ImGuiInputTextFlags.EnterReturnsTrue))
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
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public void Process(in Toggle<ITerminalService> message)
        {
            this.Visible = !this.Visible;
        }
    }
}
