using Guppy.Common;
using Guppy.MonoGame.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions
{
    public abstract class CommandDefinition : ICommandDefinition
    {
        public static Type Guppy = typeof(Commands.Guppy);

        public virtual Type? Parent { get; } = null;
        public virtual string Name => this.GetType().Name.ToLower();
        public virtual string? Description { get; }
        public virtual string[] Aliases { get; } = Array.Empty<string>();
        public IEnumerable<Option> Options => this.GetPropertyValues<Option>();
        public IEnumerable<Argument> Arguments => this.GetPropertyValues<Argument>();

        public virtual Command BuildCommand(IBus bus, ICommandService commands)
        {
            var command = new Command(this.Name, this.Description);

            foreach (string alias in this.Aliases)
            {
                command.AddAlias(alias);
            }

            foreach (Option option in this.Options)
            {
                command.AddOption(option);
            }

            foreach (Argument argument in this.Arguments)
            {
                command.AddArgument(argument);
            }

            return command;
        }

        /// <summary>
        /// Return all <typeparamref name="T"/> property values via reflection
        /// </summary>
        /// <returns></returns>
        private IEnumerable<T> GetPropertyValues<T>(BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.ExactBinding)
        {
            var properties = this.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsAssignableTo(typeof(T)))
                {
                    var value = property.GetValue(this);

                    if (value is not null)
                    {
                        yield return (T)value;
                    }
                }
            }
        }
    }

    public abstract class CommandDefinition<TData> : CommandDefinition, ICommandDefinition
        where TData : notnull, ICommandData
    {
        private class CommandDataBinder : BinderBase<TData>
        {
            private Func<BindingContext, TData> _builder;

            public CommandDataBinder(Func<BindingContext, TData> builder)
            {
                _builder = builder;
            }

            protected override TData GetBoundValue(BindingContext bindingContext)
            {
                return _builder(bindingContext);
            }
        }

        public virtual TData BindData(BindingContext context)
        {
            return default(TData)!;
        }

        public override Command BuildCommand(IBus bus, ICommandService commands)
        {
            var command = base.BuildCommand(bus, commands);

            command.SetHandler((data) =>
            {
                if (data is not null)
                {
                    bus.Publish<TData>(data);
                }
            }, new CommandDataBinder(this.BindData));

            return command;
        }
    }
}
