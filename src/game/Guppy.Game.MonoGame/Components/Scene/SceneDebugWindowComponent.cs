using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Components;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    internal sealed class SceneDebugWindowComponent : SceneComponent, IImGuiComponent
    {
        private readonly Resource<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private IDebugComponent[] _components;
        private IScene _scene;
        private ImGuiWindowClassPtr _class;

        public SceneDebugWindowComponent(IImGui imgui, IScene scene)
        {
            _scene = scene;
            _components = Array.Empty<IDebugComponent>();
            _imgui = imgui;
            _debugWindowStyle = Common.Resources.ImGuiStyles.DebugWindow;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _components = _scene.Components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void DrawImGui(GameTime gameTime)
        {
            if (Common.Settings.IsDebugWindowEnabled == false)
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
                if (_imgui.Begin($"{_scene.Name} - {_scene.Id}"))
                {
                    foreach (IDebugComponent component in _components)
                    {
                        component.RenderDebugInfo(gameTime);
                    }
                }

                _imgui.End();
            }
        }
    }
}
