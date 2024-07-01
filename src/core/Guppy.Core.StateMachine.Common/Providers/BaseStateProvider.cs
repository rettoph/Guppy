using Guppy.Core.StateMachine.Common.Enums;

namespace Guppy.Core.StateMachine.Common.Providers
{
    public abstract class BaseStateProvider : IStateProvider
    {
        public abstract bool TryGet(IStateKey key, out object? state);

        public virtual TryMatchResultEnum TryMatch(IStateKey key, object? value)
        {
            if (this.TryGet(key, out object? state) == false)
            {
                return TryMatchResultEnum.NotApplicable;
            }

            if (this.EqualityCheck(key.Type, state, value))
            {
                return TryMatchResultEnum.Matched;
            }

            return TryMatchResultEnum.NotMatched;
        }

        public virtual bool EqualityCheck(Type type, object? state, object? value)
        {
            if (state is null)
            {
                return value is null;
            }

            if (type == typeof(Type))
            {
                return TypeCompare(state as Type, value as Type);
            }

            return state.Equals(value);
        }

        private static bool TypeCompare(Type? typeA, Type? typeB)
        {
            if (typeA == typeB)
            {
                return true;
            }

            if (typeA is not null && typeA.IsAssignableTo(typeB))
            {
                return true;
            }

            return false;
        }
    }
}
