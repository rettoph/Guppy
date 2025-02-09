using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public class EngineTerminalWindowSystem(
        IImGui imgui,
        MonoGameTerminal terminal,
        ICommandService commands,
        ISettingService settingService,
        IResourceService resourceService
    ) : IEngineSystem,
        IImGuiSystem
    {
        private readonly ICommandService _commands = commands;
        private readonly IImGui _imgui = imgui;
        private readonly MonoGameTerminal _terminal = terminal;

        private string _filter = string.Empty;
        private string _input = string.Empty;
        private float _inputContainerHeight = 0;

        private int _lines;
        private bool _scrolledToBottom = true;

        private readonly SettingValue<bool> _isTerminalWindowEnabled = settingService.GetValue(Common.GuppyMonoGameSettings.IsTerminalWindowEnabled);
        private readonly Resource<ImStyle> _debugWindowStyle = resourceService.Get(Common.Resources.ImGuiStyles.DebugWindow);

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.Draw)]
        public void DrawImGui(GameTime gameTime)
        {
            if (this._isTerminalWindowEnabled == false)
            {
                return;
            }

            using (this._imgui.Apply(this._debugWindowStyle))
            {
                this._imgui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
                this._imgui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.Zero);

                ImGuiWindowClassPtr windowClass = new()
                {
                    ClassId = this._imgui.GetID(nameof(ITerminal)),
                    DockingAllowUnclassed = false
                };

                this._imgui.SetNextWindowClass(windowClass);
                this._imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                this._imgui.SetNextWindowSize(new Vector2(800, 600), ImGuiCond.FirstUseEver);
                if (this._imgui.Begin($"Engine Terminal", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoSavedSettings))
                {
                    if (this._imgui.BeginChild("#filter-container", Vector2.Zero, ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.AlwaysAutoResize | ImGuiChildFlags.AlwaysUseWindowPadding | ImGuiChildFlags.Border))
                    {
                        this._imgui.PushItemWidth(-1);
                        if (this._imgui.InputText("#filter", ref this._filter, 1 << 11, ImGuiInputTextFlags.EnterReturnsTrue))
                        {
                            this._commands.Invoke(this._filter);
                            this._input = string.Empty;
                        }
                        this._imgui.PopItemWidth();
                    }

                    this._imgui.EndChild();

                    this._imgui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(5, 5));

                    if (this._imgui.BeginChild("##output-container", new Vector2(-1, this._imgui.GetContentRegionAvail().Y - this._inputContainerHeight), ImGuiChildFlags.AlwaysUseWindowPadding, ImGuiWindowFlags.HorizontalScrollbar))
                    {
                        this._scrolledToBottom = this._imgui.GetScrollMaxY() == 0 || this._imgui.GetScrollY() / this._imgui.GetScrollMaxY() == 1;

                        foreach (MonoGameTerminalLine line in this._terminal.Lines)
                        {
                            if (this._filter == string.Empty || line.Text.Contains(this._filter, StringComparison.InvariantCultureIgnoreCase))
                            {
                                this._imgui.NewLine();

                                foreach (MonoGameTerminalSegment segment in line.Segments)
                                {
                                    this._imgui.SameLine();
                                    this._imgui.TextColored(segment.Color, segment.Text.ToString());
                                }
                            }
                        }
                    }

                    if (this._scrolledToBottom && this._lines != (this._lines = this._terminal.Lines.Count))
                    {
                        this._imgui.SetScrollHereY(1.0f);
                    }

                    this._imgui.EndChild();

                    this._imgui.PopStyleVar();

                    if (this._imgui.BeginChild("#input-container", Vector2.Zero, ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.AlwaysAutoResize | ImGuiChildFlags.AlwaysUseWindowPadding | ImGuiChildFlags.Border))
                    {
                        this._imgui.PushItemWidth(-1);
                        if (this._imgui.InputText("#input", ref this._input, 1 << 11, ImGuiInputTextFlags.EnterReturnsTrue) && this._input != string.Empty)
                        {
                            this._imgui.SetKeyboardFocusHere(-1);
                            this._commands.Invoke(this._input);
                            this._input = string.Empty;
                        }
                        this._imgui.PopItemWidth();

                        this._inputContainerHeight = this._imgui.GetWindowSize().Y;
                    }

                    this._imgui.EndChild();
                }

                this._imgui.End();

                this._imgui.PopStyleVar();
                this._imgui.PopStyleVar();
            }
        }
    }
}