using Guppy.Common.Extensions;
using Guppy.Common;
using Guppy.GUI.Styling;
using Guppy.GUI;
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
    internal class GlobalDebugWindowComponent : GlobalComponent, IGuiComponent
    {
        private readonly ResourceValue<Style> _debugWindowStyle;
        private readonly IGui _gui;
        private IDebugComponent[] _components;
        private Ref<bool> _enabled;

        public GlobalDebugWindowComponent(IGui gui, ISettingProvider settings)
        {
            _components = Array.Empty<IDebugComponent>();
            _gui = gui;
            _debugWindowStyle = gui.GetStyle(Resources.Styles.DebugWindow);

            _enabled = settings.Get(Settings.IsDebugWindowEnabled);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void DrawGui(GameTime gameTime)
        {
            if (_enabled == false)
            {
                return;
            }

            using (_gui.Apply(_debugWindowStyle))
            {
                GuiWindowClassPtr windowClass = new GuiWindowClassPtr();
                windowClass.ClassId = _gui.GetID(nameof(IDebugComponent));
                windowClass.DockNodeFlagsOverrideSet = GuiDockNodeFlags.NoDockingSplit;
                windowClass.DockingAllowUnclassed = false;

                _gui.SetNextWindowClass(windowClass);
                _gui.SetNextWindowDockID(windowClass.ClassId, GuiCond.FirstUseEver);
                if (_gui.Begin($"Game Debug Window"))
                {
                    foreach (IDebugComponent component in _components)
                    {
                        component.RenderDebugInfo(gameTime);
                    }
                }

                _gui.End();
            }
        }
    }
}
