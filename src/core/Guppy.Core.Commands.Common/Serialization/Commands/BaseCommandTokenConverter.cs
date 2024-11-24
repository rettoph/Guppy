namespace Guppy.Core.Commands.Common.Serialization.Commands
{
    public abstract class BaseCommandTokenConverter<T> : ICommandTokenConverter
    {
        bool ICommandTokenConverter.AppliesTo(Type type)
        {
            return type == typeof(T);
        }

        object? ICommandTokenConverter.Deserialize(Type type, string token)
        {
            return Deserialize(type, token);
        }

        protected abstract T Deserialize(Type type, string token);
    }
}
