using Guppy.Gaming.Definitions;
using Guppy.Gaming.Enums;
using Guppy.Gaming.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class InputService : IInputService
    {
        private IServiceProvider _provider;
        private ICommandService _commands;
        private Dictionary<string, Input> _inputs;

        private HashSet<Input> _mouseInputs;
        private HashSet<Input> _keyboardInputs;

        public InputService(IServiceProvider provider, ICommandService commands, IEnumerable<InputDefinition> definitions)
        {
            _provider = provider;
            _commands = commands;
            _mouseInputs = new HashSet<Input>();
            _keyboardInputs = new HashSet<Input>();

            _inputs = new Dictionary<string, Input>(definitions.Count());

            foreach (InputDefinition definition in definitions)
            {
                var input = definition.BuildInput(_commands);
                this.ConfigureInput(input);

                _inputs.Add(input.Key, input);
            }
        }

        private void ConfigureInput(Input input)
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

        void IInputService.Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();

            foreach(Input input in _inputs.Values)
            {
                input.UpdateState(_provider, ref kState, ref mState);
            }
        }

        private void HandeInputSourceChanged(Input input, InputSource old, InputSource value)
        {
            // The source type didnt change, so nothing needs to happen...
            if(old.Type == value.Type)
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

        public IEnumerator<Input> GetEnumerator()
        {
            return _inputs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<Input> IEnumerable<Input>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
