namespace Guppy.StateMachine.Services
{
    public interface IStateService
    {
        bool Matches<T>(IState<T> state);
        bool Matches<T>(IStateKey<T> key, T? value);
    }
}
