using Guppy.Gaming.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions.Inputs
{
    internal sealed class RuntimeInputDefinition<TOnPress, TOnRelease> : InputDefinition<TOnPress, TOnRelease>
    {
        public override string Key { get; }
        public override InputSource DefaultSource { get; }

        public readonly Func<IServiceProvider, TOnPress?> OnPress;
        public readonly Func<IServiceProvider, TOnRelease?> OnRelease;

        public RuntimeInputDefinition(string name, InputSource defaultSource, Func<IServiceProvider, TOnPress?> onPress, Func<IServiceProvider, TOnRelease?> onRelease)
        {
            this.Key = name;
            this.DefaultSource = defaultSource;
            this.OnPress = onPress;
            this.OnRelease = onRelease;
        }

        public override bool GetOnPress(IServiceProvider provider, [MaybeNullWhen(false)] out TOnPress command)
        {
            command = this.OnPress(provider);
            return command is not null;
        }

        public override bool GetOnRelease(IServiceProvider provider, [MaybeNullWhen(false)] out TOnRelease command)
        {
            command = this.OnRelease(provider);
            return command is not null;
        }
    }
}
