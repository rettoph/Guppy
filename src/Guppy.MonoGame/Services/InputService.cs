using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Implementations;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Enums;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    [GlobalScopeFilter]
    internal sealed class InputService : SimpleGameComponent, IInputService
    {
        private readonly Dictionary<string, IInput> _inputs;
        private readonly HashSet<IInput> _mouseInputs;
        private readonly HashSet<IInput> _keyboardInputs;
        private readonly IGlobalBroker _broker;

        public InputService(IGlobalBroker broker, IEnumerable<IInputDefinition> definitions)
        {
            _broker = broker;
            _mouseInputs = new HashSet<IInput>();
            _keyboardInputs = new HashSet<IInput>();

            _inputs = new Dictionary<string, IInput>(definitions.Count());

            foreach (IInputDefinition definition in definitions)
            {
                var input = definition.BuildInput();
                this.ConfigureInput(input);

                _inputs.Add(input.Key, input);
            }
        }

        private void ConfigureInput(IInput input)
        {
            input.OnSourceChanged += this.HandeInputSourceChanged;

            switch (input.Source.Type)
            {
                case InputType.Mouse:
                    _mouseInputs.Add(input);
                    break;
                case InputType.Keyboard:
                    _keyboardInputs.Add(input);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Refactor such that input states can be passed by 
            // some managing service
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();

            foreach (IInput input in _inputs.Values)
            {
                if(input.Update(ref kState, ref mState, out IMessage? data))
                {
                    _broker.Publish(data);
                }
            }
        }

        private void HandeInputSourceChanged(IInput input, InputSource old, InputSource value)
        {
            // The source type didnt change, so nothing needs to happen...
            if (old.Type == value.Type)
            {
                return;
            }

            switch (value.Type)
            {
                case InputType.Mouse:
                    _keyboardInputs.Remove(input);
                    _mouseInputs.Add(input);
                    break;
                case InputType.Keyboard:
                    _mouseInputs.Remove(input);
                    _keyboardInputs.Add(input);
                    break;
            }
        }

        public IEnumerator<IInput> GetEnumerator()
        {
            return _inputs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
