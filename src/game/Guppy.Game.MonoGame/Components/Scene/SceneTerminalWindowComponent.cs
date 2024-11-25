using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    internal class SceneTerminalWindowComponent(IImGui imgui, MonoGameTerminal terminal, ICommandService commands, IScene scene, ISettingService settingService, IResourceService resourceService) : ISceneComponent, IImGuiComponent
    {
        private readonly ICommandService _commands = commands;
        private readonly IImGui _imgui = imgui;
        private readonly MonoGameTerminal _terminal = terminal;

        private string _filter = string.Empty;
        private string _input = string.Empty;
        private float _inputContainerHeight = 0;

        private int _lines;
        private bool _scrolledToBottom = true;

        private readonly IScene _scene = scene;

        private readonly SettingValue<bool> _isTerminalWindowEnabled = settingService.GetValue(Common.Settings.IsTerminalWindowEnabled);
        private readonly Resource<ImStyle> _debugWindowStyle = resourceService.Get(Common.Resources.ImGuiStyles.DebugWindow);

        [SequenceGroup<ImGuiSequenceGroup>(ImGuiSequenceGroup.Draw)]
        public void DrawImGui(GameTime gameTime)
        {
            if (_isTerminalWindowEnabled == false)
            {
                return;
            }

            using (_imgui.Apply(_debugWindowStyle))
            {
                _imgui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
                _imgui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.Zero);

                ImGuiWindowClassPtr windowClass = new()
                {
                    ClassId = _imgui.GetID(nameof(ITerminal)),
                    DockingAllowUnclassed = false
                };

                _imgui.SetNextWindowClass(windowClass);
                _imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                _imgui.SetNextWindowSize(new Vector2(800, 600), ImGuiCond.FirstUseEver);
                if (_imgui.Begin($"Terminal:{_scene.Name} - {_scene.Id}", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoSavedSettings))
                {
                    if (_imgui.BeginChild("#filter-container", Vector2.Zero, ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.AlwaysAutoResize | ImGuiChildFlags.AlwaysUseWindowPadding | ImGuiChildFlags.Border))
                    {
                        _imgui.PushItemWidth(-1);
                        if (_imgui.InputText("#filter", ref _filter, 1 << 11, ImGuiInputTextFlags.EnterReturnsTrue))
                        {
                            _commands.Invoke(_filter);
                            _input = string.Empty;
                        }
                        _imgui.PopItemWidth();
                    }

                    _imgui.EndChild();

                    _imgui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(5, 5));

                    if (_imgui.BeginChild("##output-container", new Vector2(-1, _imgui.GetContentRegionAvail().Y - _inputContainerHeight), ImGuiChildFlags.AlwaysUseWindowPadding, ImGuiWindowFlags.HorizontalScrollbar))
                    {
                        _scrolledToBottom = _imgui.GetScrollMaxY() == 0 || _imgui.GetScrollY() / _imgui.GetScrollMaxY() == 1;

                        foreach (MonoGameTerminalLine line in _terminal.Lines)
                        {
                            if (_filter == string.Empty || line.Text.Contains(_filter, StringComparison.InvariantCultureIgnoreCase))
                            {
                                _imgui.NewLine();

                                foreach (MonoGameTerminalSegment segment in line.Segments)
                                {
                                    _imgui.SameLine();
                                    _imgui.TextColored(segment.Color, segment.Text);
                                }
                            }
                        }
                    }

                    if (_scrolledToBottom && _lines != (_lines = _terminal.Lines.Count))
                    {
                        _imgui.SetScrollHereY(1.0f);
                    }

                    _imgui.EndChild();

                    _imgui.PopStyleVar();

                    if (_imgui.BeginChild("#input-container", Vector2.Zero, ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.AlwaysAutoResize | ImGuiChildFlags.AlwaysUseWindowPadding | ImGuiChildFlags.Border))
                    {
                        _imgui.PushItemWidth(-1);
                        if (_imgui.InputText("#input", ref _input, 1 << 11, ImGuiInputTextFlags.EnterReturnsTrue) && _input != string.Empty)
                        {
                            _imgui.SetKeyboardFocusHere(-1);
                            _commands.Invoke(_input);
                            _input = string.Empty;
                        }
                        _imgui.PopItemWidth();

                        _inputContainerHeight = _imgui.GetWindowSize().Y;
                    }

                    _imgui.EndChild();
                }

                _imgui.End();

                _imgui.PopStyleVar();
                _imgui.PopStyleVar();
            }
        }
    }
}
