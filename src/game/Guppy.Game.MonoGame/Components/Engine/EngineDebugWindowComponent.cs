using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    [AutoLoad]
    internal class EngineDebugWindowComponent(IImGui imgui, IGameEngine engine, ISettingService settingService, IResourceService resourceService) : IEngineComponent, IImGuiComponent
    {
        private readonly IGameEngine _engine = engine;
        private readonly ResourceValue<ImStyle> _debugWindowStyle = resourceService.GetValue(Common.Resources.ImGuiStyles.DebugWindow);
        private readonly IImGui _imgui = imgui;
        private readonly ActionSequenceGroup<DebugSequenceGroup, GameTime> _renderDebugInfoActions = new(true);

        private readonly SettingValue<bool> _isDebugWindowEnabled = settingService.GetValue(Common.Settings.IsDebugWindowEnabled);

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            _renderDebugInfoActions.Add(_engine.Components);
        }

        [SequenceGroup<ImGuiSequenceGroup>(ImGuiSequenceGroup.Draw)]
        public void DrawImGui(GameTime gameTime)
        {
            if (_isDebugWindowEnabled == false)
            {
                return;
            }

            using (_imgui.Apply(_debugWindowStyle))
            {
                ImGuiWindowClassPtr windowClass = new()
                {
                    ClassId = _imgui.GetID(nameof(IDebugComponent)),
                    DockingAllowUnclassed = false
                };

                _imgui.SetNextWindowClass(windowClass);
                _imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                if (_imgui.Begin($"Engine Debug Window"))
                {
                    _renderDebugInfoActions.Invoke(gameTime);
                }

                _imgui.End();
            }
        }
    }
}
