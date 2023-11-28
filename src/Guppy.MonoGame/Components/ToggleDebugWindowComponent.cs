using Guppy.Attributes;
using Guppy.Common;
using Guppy.GUI.Styling;
using Guppy.GUI;
using Guppy.Input;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Messages;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Constants;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    internal class ToggleDebugWindowComponent : GameLoopComponent, IInputSubscriber<Toggle<DebugWindowComponent>>
    {
        private Ref<bool> _enabled;

        public ToggleDebugWindowComponent(ISettingProvider settings)
        {
            _enabled = settings.Get(Settings.IsDebugWindowEnabled);
        }

        public void Process(in Guid messageId, in Toggle<DebugWindowComponent> message)
        {
            _enabled.Value = !_enabled.Value;
        }
    }
}
