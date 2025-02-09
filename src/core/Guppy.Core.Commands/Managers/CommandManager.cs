using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Services;

namespace Guppy.Core.Commands.Managers
{
    public abstract class CommandManager
    {
        public abstract ICommandContext Context { get; }
        public abstract Command Command { get; }

        internal CommandManager()
        {
        }

        internal abstract void Initialize(CommandService commandService);

        public abstract bool TryParse(InvocationContext context, [MaybeNullWhen(false)] out ICommand command);

        public static CommandManager Create(ICommandContext context, ICommandTokenService tokenService)
        {
            Type type = typeof(CommandBinder<>).MakeGenericType(context.Type);
            CommandManager binder = (CommandManager)(Activator.CreateInstance(type, context, tokenService) ?? throw new NotImplementedException());

            return binder;
        }
    }

    public class CommandBinder<TCommand> : CommandManager
        where TCommand : ICommand, new()
    {
        public override ICommandContext Context { get; }
        public override Command Command { get; }
        public OptionManager<TCommand>[] Options { get; }
        public ArgumentManager<TCommand>[] Arguments { get; }

        public CommandBinder(ICommandContext<TCommand> context, ICommandTokenService tokenService)
        {
            this.Context = context;
            this.Options = OptionManager<TCommand>.CreateAll(context, tokenService);
            this.Arguments = ArgumentManager<TCommand>.CreateAll(context, tokenService);

            this.Command = new Command(context.Name, context.Description);

            foreach (OptionManager<TCommand> optionManager in this.Options)
            {
                this.Command.Add(optionManager.Option);
            }

            foreach (ArgumentManager<TCommand> argumentManager in this.Arguments)
            {
                this.Command.Add(argumentManager.Argument);
            }
        }

        public override bool TryParse(InvocationContext context, [MaybeNullWhen(false)] out ICommand command)
        {
            if (this.TryParse(context, out TCommand instance) == true)
            {
                command = instance;
                return true;
            }

            command = default;
            return false;
        }

        internal override void Initialize(CommandService commandService)
        {
            this.Command.SetHandler(context =>
            {
                if (this.TryParse(context, out TCommand command) == true)
                {
                    commandService.Invoke(command);
                }
            });

            commandService.AddCommand(this.Context.Parent, this.Command);
        }

        private bool TryParse(InvocationContext context, out TCommand command)
        {
            command = new();
            bool result = true;

            foreach (OptionManager<TCommand> option in this.Options)
            {
                result &= option.TryBind(ref command, context);
            }

            foreach (ArgumentManager<TCommand> argument in this.Arguments)
            {
                result &= argument.TryBind(ref command, context);
            }

            return result;
        }
    }
}