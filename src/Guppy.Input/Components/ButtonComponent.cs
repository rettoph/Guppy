using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Input.Providers;
using Guppy.MonoGame;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Input.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class ButtonComponent : IGuppyComponent, IUpdateableComponent
    {
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonProvider[] _providers;
        private readonly IBus _bus;

        public ButtonComponent(
            IBus bus,
            IEnumerable<IButton> inputs,
            IFiltered<IButtonProvider> providers)
        {
            _bus = bus;
            _buttons = inputs.ToDictionary(x => x.Key, x => x);
            _providers = providers.Instances.ToArray();

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
                foreach (IMessage data in provider.Update())
                {
                    _bus.Enqueue(data);
                }
            }
        }
    }
}
