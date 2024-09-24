using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Guppy.Game.MonoGame.Common.Attributes;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    [SceneHasTerminalWindowFilter]
    internal class SceneTerminalWindowComponent : ISceneComponent, IImGuiComponent
    {
        private readonly ICommandService _commands;
        private readonly IImGui _imgui;
        private readonly MonoGameTerminal _terminal;

        private string _filter;
        private string _input;
        private float _inputContainerHeight;

        private int _lines;
        private bool _scrolledToBottom;

        private IScene _scene;

        private SettingValue<bool> _isTerminalWindowEnabled;
        private ResourceValue<ImStyle> _debugWindowStyle;

        public SceneTerminalWindowComponent(IImGui imgui, MonoGameTerminal terminal, ICommandService commands, IScene scene, ISettingService settingService, IResourceService resourceService)
        {
            _commands = commands;
            _imgui = imgui;
            _terminal = terminal;
            _filter = string.Empty;
            _input = string.Empty;
            _inputContainerHeight = 0;
            _scrolledToBottom = true;
            _scene = scene;
            _isTerminalWindowEnabled = settingService.GetValue(Common.Settings.IsTerminalWindowEnabled);
            _debugWindowStyle = resourceService.GetValue(Common.Resources.ImGuiStyles.DebugWindow);
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            //
        }

        [SequenceGroup<DrawImGuiSequenceGroup>(DrawImGuiSequenceGroup.Draw)]
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

                ImGuiWindowClassPtr windowClass = new ImGuiWindowClassPtr();
                windowClass.ClassId = _imgui.GetID(nameof(ITerminal));
                windowClass.DockingAllowUnclassed = false;

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
