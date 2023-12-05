using Guppy.Attributes;
using Guppy.Commands.Messages;
using Guppy.Commands.Services;
using Guppy.Common;
using Guppy.Game.Common;
using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.Resources;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components.Guppy
{
    [AutoLoad]
    internal class TerminalWindowComponent : GuppyComponent, IGuiComponent
    {
        private readonly ICommandService _commands;
        private readonly IGui _gui;
        private readonly ResourceValue<Style> _debugWindowStyle;
        private readonly MonoGameTerminal _terminal;

        private string _filter;
        private string _input;
        private float _inputContainerHeight;

        private int _lines;
        private bool _scrolledToBottom;

        private Ref<bool> _enabled;

        private IGuppy _guppy;

        public TerminalWindowComponent(IGui gui, MonoGameTerminal terminal, ISettingProvider settings, ICommandService commands)
        {
            _commands = commands;
            _gui = gui;
            _debugWindowStyle = gui.GetStyle(Resources.Styles.DebugWindow);
            _terminal = terminal;
            _filter = string.Empty;
            _input = string.Empty;
            _inputContainerHeight = 0;
            _scrolledToBottom = true;
            _guppy = null!;

            _enabled = settings.Get(Settings.IsTerminalWindowEnabled);
        }

        public override void Initialize(IGuppy guppy)
        {
            base.Initialize(guppy);

            _guppy = guppy;
        }

        public void DrawGui(GameTime gameTime)
        {
            if (_enabled == false)
            {
                return;
            }

            using (_gui.Apply(_debugWindowStyle))
            {
                _gui.PushStyleVar(GuiStyleVar.WindowPadding, Vector2.Zero);
                _gui.PushStyleVar(GuiStyleVar.ItemSpacing, Vector2.Zero);

                GuiWindowClassPtr windowClass = new GuiWindowClassPtr();
                windowClass.ClassId = _gui.GetID(nameof(ITerminal));
                windowClass.DockingAllowUnclassed = false;

                _gui.SetNextWindowClass(windowClass);
                _gui.SetNextWindowDockID(windowClass.ClassId, GuiCond.FirstUseEver);
                _gui.SetNextWindowSize(new Vector2(800, 600), GuiCond.FirstUseEver);
                if (_gui.Begin($"Terminal:{_guppy.Name} - {_guppy.Id}", GuiWindowFlags.NoScrollbar | GuiWindowFlags.NoSavedSettings))
                {
                    if (_gui.BeginChild("#filter-container", Vector2.Zero, GuiChildFlags.AutoResizeY | GuiChildFlags.AlwaysAutoResize | GuiChildFlags.AlwaysUseWindowPadding | GuiChildFlags.Border))
                    {
                        _gui.PushItemWidth(-1);
                        if (_gui.InputText("#filter", ref _filter, 1 << 11, GuiInputTextFlags.EnterReturnsTrue))
                        {
                            _commands.Invoke(_filter);
                            _input = string.Empty;
                        }
                        _gui.PopItemWidth();
                    }

                    _gui.EndChild();

                    _gui.PushStyleVar(GuiStyleVar.WindowPadding, new Vector2(5, 5));

                    if (_gui.BeginChild("##output-container", new Vector2(-1, _gui.GetContentRegionAvail().Y - _inputContainerHeight), GuiChildFlags.AlwaysUseWindowPadding, GuiWindowFlags.HorizontalScrollbar))
                    {
                        _scrolledToBottom = _gui.GetScrollMaxY() == 0 || _gui.GetScrollY() / _gui.GetScrollMaxY() == 1;

                        foreach (MonoGameTerminalLine line in _terminal.Lines)
                        {
                            if (_filter == string.Empty || line.Text.Contains(_filter, StringComparison.InvariantCultureIgnoreCase))
                            {
                                _gui.NewLine();

                                foreach (MonoGameTerminalSegment segment in line.Segments)
                                {
                                    _gui.SameLine();
                                    _gui.TextColored(segment.Color, segment.Text);
                                }
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
                        if (_gui.InputText("#input", ref _input, 1 << 11, GuiInputTextFlags.EnterReturnsTrue) && _input != string.Empty)
                        {
                            _gui.SetKeyboardFocusHere(-1);
                            _commands.Invoke(_input);
                            _input = string.Empty;
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
