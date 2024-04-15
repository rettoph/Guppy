using Guppy.Game.Commands.Extensions;
using System.CommandLine.Invocation;
using System.Reflection;

namespace Guppy.Game.Commands
{
    public sealed class Option
    {
        private readonly Func<InvocationContext, object?> _binder;

        internal readonly SCL.Option SCL;

        public readonly PropertyInfo PropertyInfo;
        public readonly string[] Names;
        public readonly string? Description;
        public readonly bool Required;

        public Option(PropertyInfo propertyInfo, string[] names, string? description, bool required)
        {
            this.PropertyInfo = propertyInfo;
            this.Names = names;
            this.Description = description;
            this.Required = required;

            var argumentBinder = this.GetSystemOptionBinder();

            this.SCL = argumentBinder.Option;
            _binder = argumentBinder.Binder;
        }

        internal object? GetValue(InvocationContext context)
        {
            return _binder.Invoke(context);
        }
    }
}
