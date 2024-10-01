namespace Guppy.Core.Common.Attributes
{
    public class OrderAttribute(int order) : Attribute
    {
        public readonly int Order = order;
    }
}
