using Guppy.Common.Extensions;
using Guppy.Common;
using Guppy.Game.ImGui.Styling;
using Guppy.Game.ImGui;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.MonoGame.Constants;
using Guppy.Commands.Messages;
using Guppy.Resources;
using Guppy.Game.Components;
using Guppy.Game.Common.Enums;

namespace Guppy.MonoGame.Components.Game
{
    [AutoLoad]
    internal class GlobalDebugWindowComponent : GlobalComponent, IImGuiComponent
    {
        private readonly ResourceValue<ImStyle> _debugWindowStyle;
        private readonly IImGui _imgui;
        private IDebugComponent[] _components;
        private Ref<bool> _enabled;

        public GlobalDebugWindowComponent(IImGui imgui, ISettingProvider settings)
        {
            _components = Array.Empty<IDebugComponent>();
            _imgui = imgui;
            _debugWindowStyle = imgui.GetStyle(Resources.Styles.DebugWindow);

            _enabled = settings.Get(Settings.IsDebugWindowEnabled);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
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
                _imgui.SetNextWindowDockID(windowClass.ClassId, ImGuiCond.Once);
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
