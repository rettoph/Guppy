namespace Guppy.Core.Commands.Common.Attributes
{
    public class ArgumentAttribute(string? name = null, string? description = null) : Attribute
    {
        public readonly string? Name = name;
        public readonly string? Description = description;
    }
}
