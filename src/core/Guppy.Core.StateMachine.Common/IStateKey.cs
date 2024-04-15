namespace Guppy.Core.StateMachine.Common
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
