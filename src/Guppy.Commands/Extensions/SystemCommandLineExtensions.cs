using Guppy.Commands.TokenPropertySetters;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands.Extensions
{
    internal static class SystemCommandLineExtensions
    {
        internal static SCL.Command GetSystemCommand(this Command command, IBroker<ICommand> broker, ITokenPropertySetter[] tokenSetters)
        {
            SCL.Command scl = new SCL.Command(command.Name, command.Description);

            foreach(SCL.Option option in command.Options.Select(o => o.SCL))
            {
                scl.AddOption(option);
            }

            foreach (SCL.Argument argument in command.Arguments.Select(o => o.SCL))
            {
                scl.AddArgument(argument);
            }

            if (command.Type.IsAssignableTo(typeof(ICommand)))
            {
                GenericInvoker(SetHandler, command.Type, command, broker, scl, tokenSetters);
            }

            return scl;
        }

        #region Arguments
        internal record ArgumentBinder(SCL.Argument Argument, Func<InvocationContext, object?> Binder);
        internal static ArgumentBinder GetSystemArgumentBinder(this Argument argument)
        {
            return (ArgumentBinder)GenericInvoker(ArgumentFactory, argument.PropertyInfo.PropertyType, argument)!;
        }

        internal static object? GetValue(this Argument argument, InvocationContext context)
        {
            return argument.GetSystemArgumentBinder().Binder(context);
        }

        private static readonly MethodInfo ArgumentFactory = GetMethodInfo(nameof(ArgumentFactoryMethod));
        private static ArgumentBinder ArgumentFactoryMethod<T>(Argument argument)
        {
            SCL.Argument<T> arg = new Argument<T>(argument.Name, () => default!, argument.Description);
            return new ArgumentBinder(
                arg,
                bc => bc.ParseResult.GetValueForArgument(arg));
        }
        #endregion

        #region Options
        internal record OptionBinder(SCL.Option Option, Func<InvocationContext, object?> Binder);
        internal static OptionBinder GetSystemOptionBinder(this Option option)
        {
            return(OptionBinder)GenericInvoker(OptionFactory, option.PropertyInfo.PropertyType, option)!;
        }

        private static readonly MethodInfo OptionFactory = GetMethodInfo(nameof(OptionFactoryMethod));
        private static OptionBinder OptionFactoryMethod<T>(Option option)
        {
            Option<T> sclOption = new Option<T>(option.Names, option.Description)
            {
                IsRequired = option.Required
            };

            return new OptionBinder(
                sclOption,
                bc => bc.ParseResult.GetValueForOption(sclOption));
        }
        #endregion

        #region Handler
        private static readonly MethodInfo SetHandler = GetMethodInfo(nameof(SetHandlerMethod));
        private static void SetHandlerMethod<T>(Command command, IBroker<ICommand> broker, SCL.Command scl, ITokenPropertySetter[] tokenSetters)
            where T : ICommand, new()
        {
            var binder = new Binder<T>(
                command,
                tokenSetters);

            scl.SetHandler(context =>
                {
                    if(binder.TryGetBoundValue(context, out T instance))
                    {
                        broker.Publish(instance);
                    }
                }
            );
        }
        #endregion

        #region Helpers
        private static T GenericFactory<T>(Type genericType, Type typeArgument, params object?[] args)
        {
            Type constructed = genericType.MakeGenericType();
            object instance = Activator.CreateInstance(constructed, args) ?? throw new Exception();

            return (T)instance;
        }

        private static MethodInfo GetMethodInfo(string name)
        {
            return typeof(SystemCommandLineExtensions).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static) ?? throw new NotImplementedException();
        }

        private static object? GenericInvoker(MethodInfo method, Type? typeArgument, params object?[] args)
        {
            MethodInfo constructed = method.MakeGenericMethod(typeArgument ?? throw new ArgumentException());
            return constructed.Invoke(null, args);
        }
        #endregion
    }
}
