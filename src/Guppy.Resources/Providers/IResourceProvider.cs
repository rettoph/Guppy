namespace Guppy.Resources.Providers
{
    public interface IResourceProvider : IGlobalComponent
    {
        void RefreshAll();
    }
}
