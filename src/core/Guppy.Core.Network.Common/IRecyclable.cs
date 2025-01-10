namespace Guppy.Core.Network.Common
{
    public interface IRecyclable : IDisposable
    {
        void Recycle();
    }
}