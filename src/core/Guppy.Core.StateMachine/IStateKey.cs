namespace Guppy.Core.StateMachine
{
    public interface IStateKey
    {
        string Value { get; }
        Type Type { get; }
    }

    public interface IStateKey<in T> : IStateKey
    {

    }
}
