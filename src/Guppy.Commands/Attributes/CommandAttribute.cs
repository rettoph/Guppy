using Autofac;
using Guppy.Attributes;
using Guppy.Commands.Extensions;
using Guppy.Configurations;

namespace Guppy.Commands.Attributes
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

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            Command command = new Command(
                type: classType,
                parent: this.Parent,
                name: this.Name ?? classType.Name.LowerCaseFirstLetter().TrimEnd(nameof(Command)),
                description: this.Description,
                options: FactoryAttribute<Option>.GetAll(classType),
                arguments: FactoryAttribute<Argument>.GetAll(classType));

            configuration.Builder.RegisterInstance<Command>(command).SingleInstance();
        }
    }

    public class CommandAttribute<TParent> : CommandAttribute
    {
        public CommandAttribute(string? name = null, string? description = null) : base(typeof(TParent), name, description)
        {
        }
    }
}
