namespace Guppy.Commands
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
    }
}
