namespace Guppy.Common.Services
{
    public interface IAttributeService<TType, TAttribute> : IEnumerable<(Type, TAttribute[])>
        where TAttribute : Attribute
    {
        TAttribute[] this[Type type] { get; }
        ITypeService<TType> Types { get; }
        TAttribute[] Get<T>()
            where T : TType;
    }
}
