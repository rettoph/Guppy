namespace Guppy.Common.Providers
{
    public interface IAttributeProvider<TType, TAttribute> : IEnumerable<(Type, TAttribute[])>
        where TAttribute : Attribute
    {
        TAttribute[] this[Type type] { get; }
        ITypeProvider<TType> Types { get; }
        TAttribute[] Get<T>()
            where T : TType;
    }
}
