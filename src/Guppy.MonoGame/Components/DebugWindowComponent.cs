using Guppy.Attributes;
using Guppy.Common;
using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.MonoGame.Messages;
using Guppy.Providers;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Constants;
using Guppy.Resources;
using Guppy.MonoGame.Common.Enums;
using Guppy.Common.Extensions;
using Guppy.Commands.Messages;
using System.ComponentModel;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    internal sealed class DebugWindowComponent : GuppyComponent, IGuiComponent, ISubscriber<Toggle<DebugWindowComponent>>
    {
        private readonly IGuiStyle _debugWindowStyle;
        private readonly IGui _gui;
        private IDebugComponent[] _components;
        private Ref<bool> _enabled;

        public DebugWindowComponent(IGui gui, ISettingProvider settings)
        {
            _components = Array.Empty<IDebugComponent>();
            _gui = gui;
            _debugWindowStyle = gui.GetStyle(Resources.Styles.DebugWindow);

            _enabled = settings.Get(Settings.IsDebugWindowEnabled);
        }

        public override void Initialize(IGuppy guppy)
        {
            base.Initialize(guppy);

            _components = guppy.Components.OfType<IDebugComponent>().Sequence(DrawSequence.Draw).ToArray();
        }

        public void DrawGui(GameTime gameTime)
        {
            if(_enabled == false)
            {
                return;
            }

            using (_gui.Apply(_debugWindowStyle))
            {                
                if (_gui.Begin($"#{nameof(DebugWindowComponent)}"))
                {
                    foreach(IDebugComponent component in _components)
                    {
                        component.RenderDebugInfo(_gui, gameTime);
                    }
                }

                _gui.End();
            }
        }

        public void Process(in Guid messageId, in Toggle<DebugWindowComponent> message)
        {
            _enabled.Value = !_enabled.Value;
        }
    }
}
