namespace Guppy.StateMachine.Providers
{
    public interface IStateProvider
    {
        IEnumerable<IState> GetStates();
    }
}
