namespace Guppy.Common.Providers
{
    public interface IServiceFilterProvider
    {
        bool Filter(IStateProvider state, object service);
    }
}
