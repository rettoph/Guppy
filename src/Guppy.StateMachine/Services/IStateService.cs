namespace Guppy.StateMachine.Services
{
    public interface IStateService
    {
        IEnumerable<IState> GetAll();

        bool Matches<T>(IState<T> state);
        bool Matches<T>(IStateKey<T> key, T? value);
    }
}
