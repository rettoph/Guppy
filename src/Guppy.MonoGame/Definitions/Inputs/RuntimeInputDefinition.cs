using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions.Inputs
{
    internal sealed class RuntimeInputDefinition<TData> : InputDefinition<TData>
        where TData : ICommandData
    {
        public override string Key { get; }

        public override InputSource DefaultSource { get; }

        public override (ButtonState, TData)[] Data { get; }

        public RuntimeInputDefinition(string key, InputSource defaultSource, (ButtonState, TData)[] data)
        {
            this.Key = key;
            this.DefaultSource = defaultSource;
            this.Data = data;
        }
    }
}
