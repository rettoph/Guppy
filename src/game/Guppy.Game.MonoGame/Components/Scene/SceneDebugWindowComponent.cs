using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    public class SceneDebugWindowComponent(IImGui imgui, IScene scene, ISettingService settingService, IResourceService resourceService) : ISceneComponent<IScene>, IImGuiComponent
    {
        private readonly IImGui _imgui = imgui;
        private readonly ActionSequenceGroup<DebugSequenceGroupEnum, GameTime> _debugActions = new(true);
        private readonly IScene _scene = scene;
        private readonly Resource<ImStyle> _debugWindowStyle = resourceService.Get(Common.Resources.ImGuiStyles.DebugWindow);
        private readonly SettingValue<bool> _isDebugWindowEnabled = settingService.GetValue(Common.Settings.IsDebugWindowEnabled);

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IScene scene)
        {
            this._debugActions.Add(this._scene.Components);
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
                    ClassId = this._imgui.GetID(nameof(IDebugComponent)),
                    DockingAllowUnclassed = false
                };

                this._imgui.SetNextWindowClass(windowClass);
                this._imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                if (this._imgui.Begin($"{this._scene.Name} - {this._scene.Id}"))
                {
                    this._debugActions.Invoke(gameTime);
                }

                this._imgui.End();
            }
        }
    }
}