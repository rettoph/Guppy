namespace Guppy.Core.Commands.Common.Serialization.Commands
{
    public interface ICommandTokenConverter
    {
        bool AppliesTo(Type type);

        object? Deserialize(Type type, string token);
    }
}
