namespace Guppy.Core.Commands.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public readonly string[] Names;
        public readonly string? Description;
        public readonly bool Required;

        public OptionAttribute(string[]? names = null, string? description = null, bool required = false)
        {
            this.Names = names ?? Array.Empty<string>();
            this.Description = description;
            this.Required = required;
        }
    }
}
