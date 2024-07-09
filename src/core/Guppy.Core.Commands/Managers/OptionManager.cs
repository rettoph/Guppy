using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Services;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;

namespace Guppy.Core.Commands.Managers
{
    public abstract class OptionManager<TCommand>
        where TCommand : ICommand
    {
        public abstract IOptionContext Context { get; }
        public abstract Option Option { get; }

        internal OptionManager()
        {

        }

        public abstract bool TryBind(ref TCommand instance, InvocationContext invocation);

        public static OptionManager<TCommand> Create(IOptionContext context, ICommandTokenService tokenService)
        {
            Type type = typeof(OptionManager<,>).MakeGenericType(typeof(TCommand), context.PropertyInfo.PropertyType);
            OptionManager<TCommand> binder = (OptionManager<TCommand>)(Activator.CreateInstance(type, context, tokenService) ?? throw new NotImplementedException());

            return binder;
        }

        public static OptionManager<TCommand>[] CreateAll(ICommandContext<TCommand> commandContext, ICommandTokenService tokenService)
        {
            List<OptionManager<TCommand>> binders = new List<OptionManager<TCommand>>();

            IEnumerable<PropertyInfo> propertyInfos = typeof(TCommand).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.HasCustomAttribute<OptionAttribute>(true));
            foreach (IOptionContext optionContext in commandContext.Options)
            {
                OptionManager<TCommand> binder = OptionManager<TCommand>.Create(optionContext, tokenService);
                binders.Add(binder);
            }

            return binders.ToArray();
        }
    }

    public class OptionManager<TCommand, TValue> : OptionManager<TCommand>
        where TCommand : ICommand
    {
        private readonly Option<TValue> _option;
        private readonly ICommandTokenService _tokenService;

        public override IOptionContext Context { get; }
        public override Option Option => _option;

        public OptionManager(IOptionContext context, ICommandTokenService tokenService)
        {
            _tokenService = tokenService;
            _option = new Option<TValue>(context.Names, context.Description)
            {
                IsRequired = true
            };

            this.Context = context;
        }

        public override bool TryBind(ref TCommand instance, InvocationContext invocation)
        {
            try
            {
                object? value = invocation.ParseResult.GetValueForOption(_option);
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
                invocation.Console.Error.WriteLine($"{nameof(OptionManager<TCommand, TValue>)}::{nameof(TryBind)} - Error binding option value '{this.Context.Names.FirstOrDefault()}'.");
                invocation.Console.Error.WriteLine(e.Message);

                return false;
            }
        }
    }
}
