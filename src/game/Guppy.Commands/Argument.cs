using Guppy.Commands.Extensions;
using System.CommandLine.Invocation;
using System.Reflection;

namespace Guppy.Commands
{
    public sealed class Argument
    {
        private readonly Func<InvocationContext, object?> _binder;

        internal readonly SCL.Argument SCL;

        public readonly PropertyInfo PropertyInfo;
        public readonly string Name;
        public readonly string? Description;

        public Argument(PropertyInfo propertyInfo, string name, string? description)
        {
            this.PropertyInfo = propertyInfo;
            this.Name = name;
            this.Description = description;

            var argumentBinder = this.GetSystemArgumentBinder();

            this.SCL = argumentBinder.Argument;
            _binder = argumentBinder.Binder;
        }

        internal object? GetValue(InvocationContext context)
        {
            return _binder.Invoke(context);
        }
    }
}
