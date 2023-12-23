namespace Guppy.Common.Providers
{
    public interface IStateProvider
    {
        bool Matches(object? state);
    }
}
