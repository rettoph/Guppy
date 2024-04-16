using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Commands.Common.TokenPropertySetters;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;

namespace Guppy.Core.Commands.Common
{
    internal class Binder<T>
        where T : ICommand, new()
    {
        private readonly Command _command;
        private readonly Dictionary<Type, ITokenPropertySetter> _tokenSetters;
        private readonly Dictionary<Option, Func<InvocationContext, object?>> _optionBinders;
        private readonly Dictionary<Argument, Func<InvocationContext, object?>> _argumentBinders;

        public Binder(Command command, ITokenPropertySetter[] tokenSetters)
        {
            _command = command;
            _tokenSetters = new Dictionary<Type, ITokenPropertySetter>();

            _optionBinders = _command.Options.ToDictionary(x => x, x => x.GetSystemOptionBinder().Binder);
            _argumentBinders = _command.Arguments.ToDictionary(x => x, x => x.GetSystemArgumentBinder().Binder);

            _tokenSetters = command.Arguments
                .Select(x => x.PropertyInfo.PropertyType)
                .Concat(command.Options.Select(x => x.PropertyInfo.PropertyType))
                .Distinct()
                .Select(p => (p, tokenSetters.FirstOrDefault(s => s.AppliesTo(p))))
                .Where(x => x.Item2 is not null)
                .ToDictionary(x => x.Item1, x => x.Item2!);
        }

        public bool TryGetBoundValue(InvocationContext context, out T instance)
        {
            instance = new();
            bool result = true;

            foreach (Option option in _command.Options)
            {
                try
                {
                    object? value = _optionBinders[option](context);

                    if (value is null)
                    {
                        continue;
                    }

                    if (value is Token token && _tokenSetters.TryGetValue(option.PropertyInfo.PropertyType, out var setter))
                    {
                        setter.SetValue(option.PropertyInfo, instance, token);
                        continue;
                    }

                    if (value.GetType().IsAssignableTo(option.PropertyInfo.PropertyType))
                    {
                        option.PropertyInfo.SetValue(instance, value);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    context.Console.Error.WriteLine($"Error parsing option '{option.Names.FirstOrDefault()}'.");
                    context.Console.Error.WriteLine(e.Message);
                    result &= false;
                }

            }

            foreach (Argument argument in _command.Arguments)
            {
                try
                {
                    object? value = _argumentBinders[argument](context);

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
                catch (Exception e)
                {
                    context.Console.Error.WriteLine($"Error parsing argument '{argument.Name}'.");
                    context.Console.Error.WriteLine(e.Message);
                    result &= false;
                }
            }

            return result;
        }
    }
}
