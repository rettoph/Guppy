using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Input.Common
{
    public interface IInputSubscriber<TInput> : IBaseSubscriber<IInput, TInput>
        where TInput : IInput
    {
    }
}
