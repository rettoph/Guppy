using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Components
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    internal sealed class ButtonPublishComponent : EngineComponent, IUpdatableComponent
    {
        private readonly IInputService _inputs;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonService[] _providers;

        public ButtonPublishComponent(
            IInputService inputs,
            IEnumerable<IButton> buttons,
            IEnumerable<IButtonService> providers)
        {
            _inputs = inputs;
            _buttons = buttons.ToDictionary(x => x.Key, x => x);
            _providers = providers.ToArray();

            foreach (var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
        }

        [Sequence<UpdateComponentSequence>(UpdateComponentSequence.PreUpdate)]
        public void Update(GameTime gameTime)
        {
            foreach (var provider in _providers)
            {
                foreach (IInput data in provider.Update())
                {
                    _inputs.Publish(data);
                }
            }
        }
    }
}
