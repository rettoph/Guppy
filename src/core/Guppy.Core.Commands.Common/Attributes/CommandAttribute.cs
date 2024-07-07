using Autofac;
using Guppy.Core.Common.Attributes;

namespace Guppy.Core.Commands.Common.Attributes
{
    public class CommandAttribute : GuppyConfigurationAttribute
    {
        public readonly string? Name;
        public readonly string? Description;
        public readonly Type? Parent;

        public CommandAttribute(Type? parent = null, string? name = null, string? description = null)
        {
            this.Parent = parent;
            this.Name = name;
            this.Description = description;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterInstance<Command>(Command.Create(classType)).SingleInstance();
        }
    }

    public class CommandAttribute<TParent> : CommandAttribute
    {
        public CommandAttribute(string? name = null, string? description = null) : base(typeof(TParent), name, description)
        {
        }
    }
}
