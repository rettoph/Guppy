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
    internal class EngineDebugWindowComponent : IEngineComponent, IInitializableComponent, IImGuiComponent
    {
        private readonly IGameEngine _engine;
        private readonly ResourceValue<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private readonly ActionSequenceGroup<DrawDebugComponentSequenceGroup, GameTime> _renderDebugInfoActions;

        private SettingValue<bool> _isDebugWindowEnabled;

        public EngineDebugWindowComponent(IImGui imgui, IGameEngine engine, ISettingService settingService, IResourceService resourceService)
        {
            _renderDebugInfoActions = new ActionSequenceGroup<DrawDebugComponentSequenceGroup, GameTime>();
            _imgui = imgui;
            _engine = engine;
            _debugWindowStyle = resourceService.GetValue(Common.Resources.ImGuiStyles.DebugWindow);
            _isDebugWindowEnabled = settingService.GetValue(Common.Settings.IsDebugWindowEnabled);
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            _renderDebugInfoActions.Add(_engine.Components);
        }

        [SequenceGroup<DrawImGuiSequenceGroup>(DrawImGuiSequenceGroup.Draw)]
        public void DrawImGui(GameTime gameTime)
        {
            if (_isDebugWindowEnabled == false)
            {
                return;
            }

            using (_imgui.Apply(_debugWindowStyle))
            {
                ImGuiWindowClassPtr windowClass = new ImGuiWindowClassPtr();
                windowClass.ClassId = _imgui.GetID(nameof(IDebugComponent));
                windowClass.DockingAllowUnclassed = false;

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
