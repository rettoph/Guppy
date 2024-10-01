namespace Guppy.Core.Commands.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute(string[]? names = null, string? description = null, bool required = false) : Attribute
    {
        public readonly string[]? Names = names;
        public readonly string? Description = description;
        public readonly bool Required = required;
    }
}
