using Guppy.Common;
using Guppy.Input;
using Guppy.Input.Providers;
using Microsoft.Xna.Framework;

namespace Guppy.Input.Services
{
    internal sealed class ButtonService : BaseGameComponent, IButtonService
    {
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonProvider[] _providers;
        private readonly IBus _bus;

        public ButtonService(
            IBus bus,
            IEnumerable<IButton> inputs,
            ISorted<IButtonProvider> providers)
        {
            _bus = bus;
            _buttons = inputs.ToDictionary(x => x.Key, x => x);
            _providers = providers.ToArray();

            foreach (var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
        }

        public bool Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, ButtonSource source)
        {
            _buttons[key].Source = source;

            foreach(var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var provider in _providers)
            {
                foreach(IMessage data in provider.Update())
                {
                    _bus.Enqueue(data);
                }
            }
        }
    }
}
