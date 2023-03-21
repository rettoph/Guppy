using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Implementations;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Enums;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal sealed class InputService : SimpleGameComponent, IInputService
    {
        private readonly Dictionary<string, IInput> _inputs;
        private readonly IInputProvider[] _providers;
        private readonly IBus _bus;

        public InputService(
            IBus bus,
            IEnumerable<IInput> inputs,
            ISorted<IInputProvider> providers)
        {
            _bus = bus;
            _inputs = inputs.ToDictionary(x => x.Key, x => x);
            _providers = providers.ToArray();

            foreach (var provider in _providers)
            {
                provider.Clean(_inputs.Values);
            }
        }

        public void Set(string key, InputSource source)
        {
            _inputs[key].Source = source;

            foreach(var provider in _providers)
            {
                provider.Clean(_inputs.Values);
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
