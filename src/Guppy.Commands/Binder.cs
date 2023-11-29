using Guppy.Commands.Arguments;
using Guppy.Commands.Extensions;
using Guppy.Commands.TokenPropertySetters;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine;
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
        private readonly Command _command;
        private readonly Dictionary<Type, ITokenPropertySetter> _tokenSetters;

        public Binder(Command command, ITokenPropertySetter[] tokenSetters)
        {
            _command = command;
            _tokenSetters = new Dictionary<Type, ITokenPropertySetter>();

            _tokenSetters = command.Arguments
                .Select(x => x.PropertyInfo.PropertyType)
                .Concat(command.Options.Select(x => x.PropertyInfo.PropertyType))
                .Distinct()
                .Select(p => (p, tokenSetters.FirstOrDefault(s => s.AppliesTo(p))))
                .Where(x => x.Item2 is not null)
                .ToDictionary(x => x.Item1, x => x.Item2!);
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

                if(value is Token token && _tokenSetters.TryGetValue(option.PropertyInfo.PropertyType, out var setter))
                {
                    setter.SetValue(option.PropertyInfo, instance, token);
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

                if (value is Token token && _tokenSetters.TryGetValue(argument.PropertyInfo.PropertyType, out var setter))
                {
                    setter.SetValue(argument.PropertyInfo, instance, token);
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
