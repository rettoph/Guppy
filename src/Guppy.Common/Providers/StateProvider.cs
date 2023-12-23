namespace Guppy.Common.Providers
{
    internal class StateProvider : IStateProvider
    {
        private IState[] _states;

        public StateProvider(IEnumerable<IState> states)
        {
            _states = states.ToArray();
        }

        public virtual bool Matches(object? value)
        {
            foreach (IState state in _states)
            {
                if (state.Matches(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
