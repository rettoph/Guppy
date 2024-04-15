namespace Guppy.Core.Serialization.Common.Services
{
    /// <summary>
    /// Providers high level "default" values given a
    /// requested type. Useful for custom serialization
    /// fallbacks.
    /// 
    /// See <see cref="JsonSerializer"/>
    /// </summary>
    public interface IDefaultInstanceService
    {
        T Get<T>();

        object Get(Type type);
    }
}
