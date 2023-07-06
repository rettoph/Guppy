using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal class StateProvider : IStateProvider
    {    
        private Dictionary<Type, IState> _genericStates;
        private Dictionary<Type, IState> _states;

        public StateProvider(IEnumerable<IState> states)
        {
            _genericStates = new Dictionary<Type, IState>();
            foreach (IState state in states)
            {
                IEnumerable<Type> genericTypes = state.GetType().GetConstructedGenericTypes(typeof(IState<>));
                foreach(Type genericType in genericTypes)
                {
                    _genericStates.Add(genericType.GenericTypeArguments[0], state);
                }
            }

            _states = new Dictionary<Type, IState>();
            foreach (IState state in states.Except(_genericStates.Values))
            {
                foreach (Type type in state.Types)
                {
                    _states.Add(type, state);
                }
            }
        }

        public virtual bool TryGet<T>([MaybeNullWhen(false)] out T value)
        {
            if(_genericStates.TryGetValue(typeof(T), out IState? state))
            {
                value = Unsafe.As<IState<T>>(state).GetValue();
                return true;
            }

            if(_states.TryGetValue(typeof(T), out state))
            {
                value = (T)state.GetValue(typeof(T))!;
                return true;
            }

            value = default!;
            return false;
        }

        public virtual bool Matches<T>(T value)
        {
            if (_genericStates.TryGetValue(typeof(T), out IState? state))
            {
                return Unsafe.As<IState<T>>(state).Matches(value);
            }

            if (_states.TryGetValue(typeof(T), out state))
            {
                return state.Matches(typeof(T), value);
            }

            return false;
        }

        public IStateProvider Custom(IState[] states)
        {
            return new CustomStateProvider(this, states);
        }
    }
}
