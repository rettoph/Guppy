using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public class EngineDebugWindowSystem(
        IImGui imgui,
        IGameEngine engine,
        ISettingService settingService,
        IResourceService resourceService
    ) : IEngineSystem,
        IInitializeSystem<IGuppyEngine>,
        IDeinitializeSystem<IGuppyEngine>,
        IImGuiSystem
    {
        private readonly IGameEngine _engine = engine;
        private readonly Resource<ImStyle> _debugWindowStyle = resourceService.Get(Common.Resources.ImGuiStyles.DebugWindow);
        private readonly IImGui _imgui = imgui;
        private readonly ActionSequenceGroup<DebugSequenceGroupEnum, GameTime> _renderDebugInfoActions = new(true);

        private readonly SettingValue<bool> _isDebugWindowEnabled = settingService.GetValue(Common.Settings.IsDebugWindowEnabled);

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            this._renderDebugInfoActions.Add(this._engine.Systems);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Initialize)]
        public void Deinitialize(IGuppyEngine obj)
        {
            this._renderDebugInfoActions.Remove(this._engine.Systems);
        }

        [SequenceGroup<ImGuiSequenceGroupEnum>(ImGuiSequenceGroupEnum.Draw)]
        public void DrawImGui(GameTime gameTime)
        {
            if (this._isDebugWindowEnabled == false)
            {
                return;
            }

            using (this._imgui.Apply(this._debugWindowStyle))
            {
                ImGuiWindowClassPtr windowClass = new()
                {
                    ClassId = this._imgui.GetID(nameof(IDebugSystem)),
                    DockingAllowUnclassed = false
                };

                this._imgui.SetNextWindowClass(windowClass);
                this._imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                if (this._imgui.Begin($"Engine Debug Window"))
                {
                    this._renderDebugInfoActions.Invoke(gameTime);
                }

                this._imgui.End();
            }
        }
    }
}