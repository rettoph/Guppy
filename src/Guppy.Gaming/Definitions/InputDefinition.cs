using Guppy.Gaming.Services;
using Guppy.Gaming.Structs;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions
{
    public abstract class InputDefinition
    {
        public abstract string Key { get; }
        public abstract InputSource DefaultSource { get; }

        public virtual Input BuildInput(ICommandService commands)
        {
            return new Input(this.Key, this.DefaultSource, this.PublishPress, this.PublishRelease, commands);
        }

        public abstract void PublishPress(IServiceProvider provider, ICommandService broker);
        public abstract void PublishRelease(IServiceProvider provider, ICommandService broker);
    }

    public abstract class InputDefinition<TOnPress, TOnRelease> : InputDefinition
    {
        public abstract bool GetOnPress(IServiceProvider provider, [MaybeNullWhen(false)] out TOnPress command);
        public abstract bool GetOnRelease(IServiceProvider provider, [MaybeNullWhen(false)] out TOnRelease command);

        public override void PublishPress(IServiceProvider provider, ICommandService broker)
        {
            if(this.GetOnPress(provider, out var command))
            {
                broker.Publish(command);
            }
        }

        public override void PublishRelease(IServiceProvider provider, ICommandService broker)
        {
            if (this.GetOnRelease(provider, out var command))
            {
                broker.Publish(command);
            }
        }
    }

    public abstract class InputDefinition<T> : InputDefinition<T, T>
    {

    }
}
