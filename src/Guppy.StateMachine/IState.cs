namespace Guppy.StateMachine
{
    public interface IState
    {
        IStateKey Key { get; }
        object? Value { get; }
    }

    public interface IState<T> : IState
    {
        new IStateKey<T> Key { get; }
        new T? Value { get; }

        bool Equals(T? other);
    }
}
