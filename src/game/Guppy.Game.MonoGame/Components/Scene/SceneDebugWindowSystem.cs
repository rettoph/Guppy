using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Scene
{
    public class SceneDebugWindowSystem(
        IImGui imgui,
        IScene scene,
        ISettingService settingService,
        IAssetService assetService
    ) : ISceneSystem,
        IInitializeSystem,
        IDeinitializeSystem,
        IImGuiSystem
    {
        private readonly IImGui _imgui = imgui;
        private readonly ActionSequenceGroup<DebugSequenceGroupEnum, GameTime> _debugActions = new(true);
        private readonly IScene _scene = scene;
        private readonly Asset<ImStyle> _debugWindowStyle = assetService.Get(Common.Assets.ImGuiStyles.DebugWindow);
        private readonly SettingValue<bool> _isDebugWindowEnabled = settingService.GetValue(Common.GuppyMonoGameSettings.IsDebugWindowEnabled);

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize()
        {
            this._debugActions.Add(this._scene.Systems.GetAll());
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.Initialize)]
        public void Deinitialize()
        {
            this._debugActions.Remove(this._scene.Systems.GetAll());
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
                if (this._imgui.Begin($"{this._scene.Name} - {this._scene.Id}"))
                {
                    this._debugActions.Invoke(gameTime);
                }

                this._imgui.End();
            }
        }
    }
}