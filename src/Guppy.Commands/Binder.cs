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
                object? value = bindingContext.ParseResult.GetValueForOption(option.GetSystemOption());

                if(value is null)
                {
                    continue;
                }

                if(value is not Token token)
                {
                    continue;
                }

                option.PropertyInfo.SetValue(instance, token.Value);
            }

            return instance;
        }
    }
}
