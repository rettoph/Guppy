using Guppy.Common;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Enums;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal sealed class InputService : IInputService
    {
        private Dictionary<string, IInput> _inputs;

        private HashSet<IInput> _mouseInputs;
        private HashSet<IInput> _keyboardInputs;

        public InputService(IGlobal<IBus> bus, IEnumerable<IInputDefinition> definitions)
        {
            _mouseInputs = new HashSet<IInput>();
            _keyboardInputs = new HashSet<IInput>();

            _inputs = new Dictionary<string, IInput>(definitions.Count());

            foreach (IInputDefinition definition in definitions)
            {
                var input = definition.BuildInput(bus);
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

        public void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();

            foreach (IInput input in _inputs.Values)
            {
                input.Update(ref kState, ref mState);
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
