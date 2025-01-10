namespace Guppy.Core.Commands.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute(Type? parent = null, string? name = null, string? description = null) : Attribute
    {
        public readonly string? Name = name;
        public readonly string? Description = description;
        public readonly Type? Parent = parent;
    }

    public class CommandAttribute<TParent>(string? name = null, string? description = null) : CommandAttribute(typeof(TParent), name, description)
    {
    }
}