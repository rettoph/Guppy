using Guppy.Messaging;

namespace Guppy.Game.Input
{
    public interface IInputSubscriber<TInput> : IBaseSubscriber<IInput, TInput>
        where TInput : IInput
    {
    }
}
