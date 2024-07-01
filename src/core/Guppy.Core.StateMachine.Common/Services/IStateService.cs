namespace Guppy.Core.StateMachine.Common.Services
{
    public interface IStateService
    {
        T? GetByKey<T>(IStateKey<T> key);
        bool Matches<T>(IStateKey<T> key, T value);
    }
}
