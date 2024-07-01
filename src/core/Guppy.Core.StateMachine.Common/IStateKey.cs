namespace Guppy.Core.StateMachine.Common
{
    public interface IStateKey
    {
        string Value { get; }
        Type Type { get; }

        public bool Equals(Type type, string value);
        public bool Equals(IStateKey key);
    }

    public interface IStateKey<T> : IStateKey
    {

    }
}
