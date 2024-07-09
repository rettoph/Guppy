using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;

namespace Guppy.Core.Commands.Managers
{
    public abstract class ArgumentManager<TCommand>
        where TCommand : ICommand
    {
        public abstract IArgumentContext Context { get; }
        public abstract Argument Argument { get; }

        internal ArgumentManager()
        {
        }

        public abstract bool TryBind(ref TCommand instance, InvocationContext invocation);

        public static ArgumentManager<TCommand> Create(IArgumentContext context, ICommandTokenService tokenService)
        {
            Type type = typeof(ArgumentManager<,>).MakeGenericType(typeof(TCommand), context.PropertyInfo.PropertyType);
            ArgumentManager<TCommand> binder = (ArgumentManager<TCommand>)(Activator.CreateInstance(type, context, tokenService) ?? throw new NotImplementedException());

            return binder;
        }

        public static ArgumentManager<TCommand>[] CreateAll(ICommandContext<TCommand> commandContext, ICommandTokenService tokenService)
        {
            List<ArgumentManager<TCommand>> binders = new List<ArgumentManager<TCommand>>();

            IEnumerable<PropertyInfo> propertyInfos = typeof(TCommand).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.HasCustomAttribute<ArgumentAttribute>(true));
            foreach (IArgumentContext argumentContext in commandContext.Arguments)
            {
                ArgumentManager<TCommand> binder = ArgumentManager<TCommand>.Create(argumentContext, tokenService);
                binders.Add(binder);
            }

            return binders.ToArray();
        }
    }

    public class ArgumentManager<TCommand, TValue> : ArgumentManager<TCommand>
        where TCommand : ICommand
    {
        private readonly Argument<TValue> _argument;
        private readonly ICommandTokenService _tokenService;

        public override IArgumentContext Context { get; }
        public override Argument Argument => _argument;

        public ArgumentManager(IArgumentContext context, ICommandTokenService tokenService)
        {
            ThrowIf.Type.IsNotAssignableFrom<TValue>(context.PropertyInfo.PropertyType);

            _tokenService = tokenService;
            _argument = new Argument<TValue>(context.Name, () => default!, context.Description);

            this.Context = context;
        }

        public override bool TryBind(ref TCommand instance, InvocationContext invocation)
        {
            try
            {
                object? value = invocation.ParseResult.GetValueForArgument(_argument);
                if (value is null)
                {
                    return true;
                }

                if (value is Token token)
                {
                    value = _tokenService.Deserialize(typeof(TValue), token.Value);
                }

                if (value is null)
                {
                    return true;
                }

                this.Context.PropertyInfo.SetValue(instance, value);
                return true;
            }
            catch (Exception e)
            {
                invocation.Console.Error.WriteLine($"{nameof(ArgumentManager<TCommand, TValue>)}::{nameof(TryBind)} - Error binding argument value '{this.Context.Name.FirstOrDefault()}'.");
                invocation.Console.Error.WriteLine(e.Message);

                return false;
            }
        }
    }
}
