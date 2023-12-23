using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Game.Common.Enums;
using Guppy.Game.Components;
using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Guppy
{
    [AutoLoad]
    internal sealed class GuppyDebugWindowComponent : GuppyComponent, IImGuiComponent
    {
        private readonly ResourceValue<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private IDebugComponent[] _components;
        private Ref<bool> _enabled;
        private IGuppy _guppy;
        private ImGuiWindowClassPtr _class;

        public GuppyDebugWindowComponent(IImGui imgui, ISettingProvider settings)
        {
            _guppy = null!;
            _components = Array.Empty<IDebugComponent>();
            _imgui = imgui;
            _debugWindowStyle = imgui.GetStyle(Resources.ImGuiStyles.DebugWindow);

            _enabled = settings.Get(Settings.IsDebugWindowEnabled);
        }

        public override void Initialize(IGuppy guppy)
        {
            base.Initialize(guppy);

            _guppy = guppy;
            _components = guppy.Components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void DrawImGui(GameTime gameTime)
        {
            if (_enabled == false)
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
                if (_imgui.Begin($"{_guppy.Name} - {_guppy.Id}"))
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
