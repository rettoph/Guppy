using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands.Extensions
{
    internal static class SystemCommandLineExtensions
    {
        internal static SCL.Command GetSystemCommand(this Command command, IBus bus)
        {
            SCL.Command scl = new SCL.Command(command.Name, command.Description);

            foreach(SCL.Option option in command.Options.Select(o => o.GetSystemOption()))
            {
                scl.AddOption(option);
            }

            if(command.Type.IsAssignableTo(typeof(IMessage)))
            {
                GenericInvoker(SetHandler, command.Type, command, bus, scl);
            }

            return scl;
        }

        #region Options
        private static Dictionary<Option, SCL.Option> _options = new Dictionary<Option, SCL.Option>();
        internal static SCL.Option GetSystemOption(this Option option)
        {
            if(!_options.TryGetValue(option, out SCL.Option? scl))
            {
                scl = (SCL.Option)GenericInvoker(OptionFactory, option.PropertyInfo.DeclaringType, option)!;
                _options.Add(option, scl);
            }

            return scl;
        }

        private static readonly MethodInfo OptionFactory = GetMethodInfo(nameof(OptionFactoryMethod));
        private static Option<T> OptionFactoryMethod<T>(Option option)
        {
            return new Option<T>(option.Name, option.Description)
            {
                IsRequired = option.Required
            };
        }
        #endregion

        #region Handler
        private static readonly MethodInfo SetHandler = GetMethodInfo(nameof(SetHandlerMethod));
        private static void SetHandlerMethod<T>(Command command, IBus bus, SCL.Command scl)
            where T : IMessage
        {
            scl.SetHandler(value =>
            {
                bus.Enqueue(value);
            }, new Binder<T>(command));
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
