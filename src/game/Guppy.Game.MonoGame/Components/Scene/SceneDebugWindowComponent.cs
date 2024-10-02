﻿using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Attributes;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Enums;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    [SceneHasDebugWindowFilter]
    internal sealed class SceneDebugWindowComponent(IImGui imgui, IScene scene, ISettingService settingService, IResourceService resourceService) : ISceneComponent<IScene>, IImGuiComponent
    {
        private readonly IImGui _imgui = imgui;
        private readonly ActionSequenceGroup<DebugSequenceGroup, GameTime> _debugActions = new();
        private readonly IScene _scene = scene;
        private readonly ResourceValue<ImStyle> _debugWindowStyle = resourceService.GetValue(Common.Resources.ImGuiStyles.DebugWindow);
        private readonly SettingValue<bool> _isDebugWindowEnabled = settingService.GetValue(Common.Settings.IsDebugWindowEnabled);

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            _debugActions.Add(_scene.Components);
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
                ImGuiWindowClassPtr windowClass = new();
                windowClass.ClassId = _imgui.GetID(nameof(IDebugComponent));
                windowClass.DockingAllowUnclassed = false;

                _imgui.SetNextWindowClass(windowClass);
                _imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.FirstUseEver);
                if (_imgui.Begin($"{_scene.Name} - {_scene.Id}"))
                {
                    _debugActions.Invoke(gameTime);
                }

                _imgui.End();
            }
        }
    }
}
