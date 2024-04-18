using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Components;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    internal class EngineDebugWindowComponent : EngineComponent, IImGuiComponent
    {
        private readonly IGameEngine _engine;
        private readonly Resource<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private IDebugComponent[] _components;

        public EngineDebugWindowComponent(IImGui imgui, IGameEngine engine)
        {
            _components = Array.Empty<IDebugComponent>();
            _imgui = imgui;
            _engine = engine;
            _debugWindowStyle = Common.Resources.ImGuiStyles.DebugWindow;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _components = _engine.Components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
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
