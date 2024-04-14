using Guppy.Engine;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Extensions;
using Guppy.Game.Common.Enums;
using Guppy.Game.Components;
using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using Guppy.Resources.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    internal class GlobalDebugWindowComponent : GlobalComponent, IImGuiComponent
    {
        private readonly Resource<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private IDebugComponent[] _components;

        public GlobalDebugWindowComponent(IImGui imgui, ISettingService settings)
        {
            _components = Array.Empty<IDebugComponent>();
            _imgui = imgui;
            _debugWindowStyle = Resources.ImGuiStyles.DebugWindow;
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void DrawImGui(GameTime gameTime)
        {
            if (Settings.IsDebugWindowEnabled == false)
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
                if (_imgui.Begin($"Game Debug Window"))
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
