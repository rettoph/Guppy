using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Common;
using System.Reflection;

namespace Guppy.Core.Commands.Common.Contexts
{
    public abstract class CommandContext : ICommandContext
    {
        public abstract Type Type { get; }
        public abstract string Name { get; }
        public abstract string? Description { get; }
        public abstract Type? Parent { get; }
        public abstract IOptionContext[] Options { get; }
        public abstract IArgumentContext[] Arguments { get; }

        internal CommandContext()
        {

        }

        public static ICommandContext Create(Type type, Type? parent = null, string? name = null, string? description = null)
        {
            ThrowIf.Type.IsNotAssignableFrom<ICommand>(type);

            CommandAttribute? attribute = type.GetCustomAttribute<CommandAttribute>();
            parent ??= attribute?.Parent;
            name ??= attribute?.Name ?? type.Name.ToCommandName();
            description ??= attribute?.Description;

            Type contextType = typeof(CommandContext<>).MakeGenericType(type);
            ICommandContext context = (ICommandContext)(Activator.CreateInstance(contextType, parent, name, description) ?? throw new NotImplementedException());

            return context;
        }
    }

    public class CommandContext<T>(
        Type? parent,
        string name,
        string? description) : CommandContext, ICommandContext<T>
        where T : ICommand, new()
    {
        public override Type Type => typeof(T);

        public override string Name { get; } = name;

        public override string? Description { get; } = description;

        public override Type? Parent { get; } = parent;

        public override IOptionContext[] Options { get; } = OptionContext.CreateAll<T>();

        public override IArgumentContext[] Arguments { get; } = ArgumentContext.CreateAll<T>();
    }
}
