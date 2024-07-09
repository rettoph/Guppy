namespace Guppy.Core.Commands.Common.Services
{
    public interface ICommandTokenService
    {
        object? Deserialize(Type type, string token);
    }
}
