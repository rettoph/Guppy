namespace Guppy.Core.StateMachine.Common.Services
{
    public interface IStateService
    {
        IState<T> GetByKey<T>(IStateKey<T> key);

        IEnumerable<IState> GetAll();

        bool Matches<T>(IState<T> state);
        bool Matches<T>(IStateKey<T> key, T? value);
    }
}
