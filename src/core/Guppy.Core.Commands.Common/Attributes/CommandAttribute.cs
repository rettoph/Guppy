using Autofac;
using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Commands.Common.Attributes
{
    public class CommandAttribute(Type? parent = null, string? name = null, string? description = null) : GuppyConfigurationAttribute
    {
        public readonly string? Name = name;
        public readonly string? Description = description;
        public readonly Type? Parent = parent;

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

    public class CommandAttribute<TParent>(string? name = null, string? description = null) : CommandAttribute(typeof(TParent), name, description)
    {
    }
}
