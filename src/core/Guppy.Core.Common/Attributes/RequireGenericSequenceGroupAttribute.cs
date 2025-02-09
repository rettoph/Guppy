namespace Guppy.Core.Common.Attributes
{
    public sealed class RequireGenericSequenceGroupAttribute(string genericArgumentName) : Attribute
    {
        public readonly string GenericArgumentName = genericArgumentName;
    }
}
