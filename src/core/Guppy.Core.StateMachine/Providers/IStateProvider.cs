namespace Guppy.Core.StateMachine.Providers
{
    public interface IStateProvider
    {
        IEnumerable<IState> GetStates();
    }
}
