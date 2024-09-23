using Autofac;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.System.Reflection;

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
            ICommandContext context = CommandContext.Create(classType);
            Type contextType = context.GetType();

            foreach (GuppyConfigurationAttribute attribute in classType.GetAllCustomAttributes<GuppyConfigurationAttribute>(true))
            {
                if (attribute is CommandAttribute)
                { // Ignore the existing command attribute
                    continue;
                }

                // Pass existing attributes on the command data
                // type to the created context type
                attribute.TryConfigure(boot, builder, contextType);
            }

            builder.RegisterInstance(context).As<ICommandContext>().SingleInstance();
        }
    }

    public class CommandAttribute<TParent> : CommandAttribute
    {
        public CommandAttribute(string? name = null, string? description = null) : base(typeof(TParent), name, description)
        {
        }
    }
}
