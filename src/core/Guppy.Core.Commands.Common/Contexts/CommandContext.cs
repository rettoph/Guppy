using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
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

        public static ICommandContext Create(ICommand defaultInstance)
        {
            Type contextType = typeof(CommandContext<>).MakeGenericType(defaultInstance.GetType());
            ICommandContext context = (ICommandContext)(Activator.CreateInstance(contextType, defaultInstance) ?? throw new NotImplementedException());

            return context;
        }
    }

    public class CommandContext<T> : CommandContext, ICommandContext<T>
        where T : ICommand, new()
    {
        private readonly T _defaultInstance;

        public override Type Type => typeof(T);

        public override string Name { get; }

        public override string? Description { get; }

        public override Type? Parent { get; }

        public override IOptionContext[] Options { get; } = OptionContext.CreateAll<T>();

        public override IArgumentContext[] Arguments { get; } = ArgumentContext.CreateAll<T>();

        public CommandContext(T defaultInstance)
        {
            _defaultInstance = defaultInstance;

            CommandAttribute? attribute = typeof(T).GetCustomAttribute<CommandAttribute>();
            this.Parent ??= attribute?.Parent;
            this.Name ??= attribute?.Name ?? typeof(T).Name.ToCommandName();
            this.Description ??= attribute?.Description;
        }
    }
}
