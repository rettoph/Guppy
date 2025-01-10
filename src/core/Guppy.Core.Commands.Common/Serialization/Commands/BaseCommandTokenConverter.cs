namespace Guppy.Core.Commands.Common.Serialization.Commands
{
    public abstract class BaseCommandTokenConverter<T> : ICommandTokenConverter
    {
        bool ICommandTokenConverter.AppliesTo(Type type) => type == typeof(T);

        object? ICommandTokenConverter.Deserialize(Type type, string token) => this.Deserialize(type, token);

        protected abstract T Deserialize(Type type, string token);
    }
}