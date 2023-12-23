using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Providers;
using Guppy.Game.Input.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class ButtonPublishComponent : GlobalComponent, IGuppyUpdateable
    {
        private readonly IInputService _inputs;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonProvider[] _providers;

        public ButtonPublishComponent(
            IInputService inputs,
            IEnumerable<IButton> buttons,
            IEnumerable<IButtonProvider> providers)
        {
            _inputs = inputs;
            _buttons = buttons.ToDictionary(x => x.Key, x => x);
            _providers = providers.ToArray();

            foreach (var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
        }

        public void Initialize(IGuppy guppy)
        {
            //
        }

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
