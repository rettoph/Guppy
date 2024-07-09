namespace Guppy.Core.Commands.Common.Attributes
{
    public class ArgumentAttribute : Attribute
    {
        public readonly string? Name;
        public readonly string? Description;

        public ArgumentAttribute(string? name = null, string? description = null)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
