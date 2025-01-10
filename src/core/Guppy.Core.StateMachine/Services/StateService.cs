using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Enums;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Core.StateMachine.Common.Services;

namespace Guppy.StateMachine.Services
{
    internal class StateService(IEnumerable<IStateProvider> providers) : IStateService
    {
        private readonly IStateProvider[] _providers = providers.ToArray();

        public T? GetByKey<T>(IStateKey<T> key)
        {
            foreach (IStateProvider provider in this._providers)
            {
                if (provider.TryGet(key, out object? state) == false)
                {
                    continue;
                }

                if (state is not T casted)
                {
                    continue;
                }

                return casted;
            }

            return default;
        }

        public bool Matches<T>(IStateKey<T> key, T value)
        {
            foreach (IStateProvider provider in this._providers)
            {
                TryMatchResultEnum result = provider.TryMatch(key, value);
                switch (result)
                {
                    case TryMatchResultEnum.NotApplicable:
                        continue;
                    case TryMatchResultEnum.NotMatched:
                        return false;
                    case TryMatchResultEnum.Matched:
                        return true;
                }
            }

            return false;
        }
    }
}