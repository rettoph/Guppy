namespace Guppy.Core.Commands.Common.Contexts
{
    public interface ICommandContext
    {
        Type Type { get; }
        string Name { get; }
        string? Description { get; }
        Type? Parent { get; }
        IOptionContext[] Options { get; }
        IArgumentContext[] Arguments { get; }
    }

    public interface ICommandContext<T> : ICommandContext
        where T : ICommand
    {

    }
}