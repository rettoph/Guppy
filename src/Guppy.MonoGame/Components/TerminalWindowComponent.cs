using Guppy.Attributes;
using Guppy.GUI;
using Guppy.GUI.Styling;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    [GuppyFilter<GameLoop>]
    internal class TerminalWindowComponent : GuppyComponent, IGuiComponent
    {
        private readonly IGui _gui;
        private readonly IGuiStyle _debugWindowStyle;
        private readonly Terminal _terminal;

        private string _input;
        private float _inputContainerHeight;

        private int _lines;
        private bool _scrolledToBottom;

        public TerminalWindowComponent(IGui gui, Terminal terminal)
        {
            _gui = gui;
            _debugWindowStyle = gui.GetStyle(Resources.Styles.DebugWindow);
            _terminal = terminal;
            _input = string.Empty;
            _inputContainerHeight = 0;
            _scrolledToBottom = true;
        }

        public void DrawGui(GameTime gameTime)
        {
            Vector2 viewport = _gui.GetMainViewport().Size;
            _gui.SetNextWindowPos(Vector2.Zero);
            _gui.SetNextWindowSize(viewport);

            using (_gui.Apply(_debugWindowStyle))
            {
                _gui.PushStyleVar(GuiStyleVar.WindowPadding, Vector2.Zero);
                _gui.PushStyleVar(GuiStyleVar.ItemSpacing, Vector2.Zero);

                if (_gui.Begin($"#{nameof(TerminalWindowComponent)}", GuiWindowFlags.NoResize | GuiWindowFlags.NoMove | GuiWindowFlags.NoTitleBar))
                {
                    _gui.PushStyleVar(GuiStyleVar.WindowPadding, new Vector2(5, 5));

                    if (_gui.BeginChild("#output-container", new Vector2(-1, viewport.Y - _inputContainerHeight), GuiChildFlags.AlwaysUseWindowPadding, GuiWindowFlags.HorizontalScrollbar))
                    {
                        _scrolledToBottom = _gui.GetScrollMaxY() == 0 || _gui.GetScrollY() / _gui.GetScrollMaxY() == 1;

                        foreach (TerminalLine line in _terminal.Lines)
                        {
                            _gui.NewLine();
                            foreach (TerminalSegment segment in line.Segments)
                            {
                                _gui.SameLine();
                                _gui.TextColored(segment.ForegroundColor, segment.Text);
                            }
                        }
                    }

                    if (_scrolledToBottom && _lines != (_lines = _terminal.Lines.Count))
                    {
                        _gui.SetScrollHereY(1.0f);
                    }

                    _gui.EndChild();

                    _gui.PopStyleVar();

                    if (_gui.BeginChild("#input-container", Vector2.Zero, GuiChildFlags.AutoResizeY | GuiChildFlags.AlwaysAutoResize | GuiChildFlags.AlwaysUseWindowPadding | GuiChildFlags.Border))
                    {
                        _gui.PushItemWidth(-1);
                        if(_gui.InputText("#input", ref _input, 1 << 11, GuiInputTextFlags.EnterReturnsTrue))
                        {
                            throw new NotImplementedException();
                        }
                        _gui.PopItemWidth();

                        _inputContainerHeight = _gui.GetWindowSize().Y;
                    }

                    _gui.EndChild();
                }

                _gui.End();

                _gui.PopStyleVar();
                _gui.PopStyleVar();
            }
        }
    }
}
