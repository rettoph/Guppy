namespace Guppy.Game.Input.Common.Services
{
    public interface IInputService
    {
        void Publish<TInput>(TInput input)
            where TInput : IInputMessage;
    }
}