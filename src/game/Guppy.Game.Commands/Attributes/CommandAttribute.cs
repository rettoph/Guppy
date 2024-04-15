using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Game.Commands.Extensions;

namespace Guppy.Game.Commands.Attributes
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

        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            Command command = new Command(
                type: classType,
                parent: this.Parent,
                name: this.Name ?? classType.Name.LowerCaseFirstLetter().TrimEnd(nameof(Command)),
                description: this.Description,
                options: FactoryAttribute<Option>.GetAll(classType),
                arguments: FactoryAttribute<Argument>.GetAll(classType));

            builder.RegisterInstance<Command>(command).SingleInstance();
        }
    }

    public class CommandAttribute<TParent> : CommandAttribute
    {
        public CommandAttribute(string? name = null, string? description = null) : base(typeof(TParent), name, description)
        {
        }
    }
}
