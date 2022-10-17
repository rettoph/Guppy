using Guppy.Common;
using Guppy.MonoGame.Services;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions
{
    public abstract class InputDefinition<TData> : IInputDefinition
        where TData : ICommandData
    {
        public abstract string Key { get; }

        public abstract InputSource DefaultSource { get; }

        public abstract (ButtonState, TData)[] Data { get; }

        public IInput BuildInput(IGlobal<IBus> bus)
        {
            return new Input<TData>(this.Key, this.DefaultSource, this.Data, bus);
        }
    }
}
