using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Input.Providers;
using Guppy.Input.Services;
using Guppy.MonoGame;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Input.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class ButtonPublishComponent : GlobalComponent, IUpdateableComponent
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

        public bool Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, ButtonSource source)
        {
            _buttons[key].Source = source;

            foreach (var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
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

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
