﻿using Guppy.Attributes;
using Guppy.Commands.Messages;
using Guppy.Commands.Services;
using Guppy.Common;
using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    internal class TerminalWindowComponent : GuppyComponent, IGuiComponent, ISubscriber<Toggle<TerminalWindowComponent>>
    {
        private readonly ICommandService _commands;
        private readonly IGui _gui;
        private readonly IGuiStyle _debugWindowStyle;
        private readonly Terminal _terminal;

        private string _input;
        private float _inputContainerHeight;

        private int _lines;
        private bool _scrolledToBottom;

        private Ref<bool> _enabled;

        private IGuppy _guppy;

        public TerminalWindowComponent(IGui gui, Terminal terminal, ISettingProvider settings, ICommandService commands)
        {
            _commands = commands;
            _gui = gui;
            _debugWindowStyle = gui.GetStyle(Resources.Styles.DebugWindow);
            _terminal = terminal;
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

                if (_gui.Begin(_guppy.Name))
                {
                    _gui.PushStyleVar(GuiStyleVar.WindowPadding, new Vector2(5, 5));

                    if (_gui.BeginChild("##output-container", new Vector2(-1, _gui.GetWindowContentRegionMax().Y - _inputContainerHeight), GuiChildFlags.AlwaysUseWindowPadding, GuiWindowFlags.HorizontalScrollbar))
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

        public void Process(in Guid messageId, in Toggle<TerminalWindowComponent> message)
        {
            _enabled.Value = !_enabled.Value;
        }
    }
}
