using System.Reflection;

namespace Guppy.Core.Commands.Common.Contexts
{
    public interface IOptionContext
    {
        string[] Names { get; }
        string? Description { get; }
        bool Required { get; }
        PropertyInfo PropertyInfo { get; }
    }

    public interface IOptionContext<TCommand> : IOptionContext
        where TCommand : ICommand
    {
    }
}