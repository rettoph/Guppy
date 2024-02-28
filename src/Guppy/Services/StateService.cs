using Guppy.StateMachine;
using Guppy.StateMachine.Providers;
using Guppy.StateMachine.Services;

namespace Guppy.Services
{
    internal class StateService : IStateService
    {
        private readonly Dictionary<IStateKey, IState> _states;

        public StateService(IEnumerable<IStateProvider> states)
        {
            _states = states.SelectMany(x => x.GetStates()).ToDictionary(x => x.Key, x => x);
        }

        public IEnumerable<IState> GetAll()
        {
            return _states.Values;
        }

        public IState<T> GetByKey<T>(IStateKey<T> key)
        {
            return _states[key] as IState<T> ?? throw new Exception();
        }

        public bool Matches<T>(IStateKey<T> key, T? value)
        {
            if (_states.TryGetValue(key, out IState? state) == false)
            {
                return false;
            }

            if (state is not IState<T> casted)
            {
                return false;
            }

            return casted.Equals(value);
        }

        public bool Matches<T>(IState<T> state)
        {
            return this.Matches(state.Key, state.Value);
        }
    }
}
