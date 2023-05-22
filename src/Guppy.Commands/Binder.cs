using Guppy.Commands.Arguments;
using Guppy.Commands.Extensions;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands
{
    internal class Binder<T> : BinderBase<T>
        where T : IMessage
    {
        private Command _command;

        public Binder(Command command)
        {
            _command = command;
        }

        protected override T GetBoundValue(BindingContext bindingContext)
        {
            T instance = Activator.CreateInstance<T>();

            foreach(Option option in _command.Options)
            {
                SCL.Option sclOption = option.GetSystemOption();
                object? value = bindingContext.ParseResult.GetValueForOption(sclOption);

                if(value is null)
                {
                    continue;
                }

                if(value is Token token)
                {
                    option.PropertyInfo.SetValue(instance, token.Value);
                    continue;
                }

                if(value.GetType().IsAssignableTo(option.PropertyInfo.PropertyType))
                {
                    option.PropertyInfo.SetValue(instance, value);
                    continue;
                }
            }

            foreach (Argument argument in _command.Arguments)
            {
                SCL.Argument sclArgument = argument.GetSystemArgument();
                object? value = bindingContext.ParseResult.GetValueForArgument(sclArgument);

                if (value is null)
                {
                    continue;
                }

                if (value is Token token)
                {
                    argument.PropertyInfo.SetValue(instance, token.Value);
                    continue;
                }

                if (value.GetType().IsAssignableTo(argument.PropertyInfo.PropertyType))
                {
                    argument.PropertyInfo.SetValue(instance, value);
                    continue;
                }
            }

            return instance;
        }
    }
}
