using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Common;
using System.Reflection;

namespace Guppy.Core.Commands.Common
{
    public sealed class Command
    {
        public readonly Type Type;
        public readonly Type? Parent;
        public readonly string Name;
        public readonly string? Description;
        public readonly Option[] Options;
        public readonly Argument[] Arguments;

        public Command(
            Type type,
            Type? parent,
            string name,
            string? description,
            Option[] options,
            Argument[] arguments)
        {
            this.Type = type;
            this.Parent = parent;
            this.Name = name;
            this.Description = description;
            this.Options = options;
            this.Arguments = arguments;
        }

        public static Command Create(Type classType)
        {
            ThrowIf.Type.IsNotAssignableFrom<ICommand>(classType);

            CommandAttribute commandAttribute = classType.GetCustomAttribute<CommandAttribute>(true) ?? throw new NotImplementedException();

            Command command = new Command(
                type: classType,
                parent: commandAttribute.Parent,
                name: commandAttribute.Name ?? classType.Name.LowerCaseFirstLetter().TrimEnd(nameof(Command)),
                description: commandAttribute.Description,
                options: FactoryAttribute<Option>.GetAll(classType),
                arguments: FactoryAttribute<Argument>.GetAll(classType));

            return command;
        }
    }
}
