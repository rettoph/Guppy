namespace Guppy.Core.Network
{
    public interface IRecyclable : IDisposable
    {
        void Recycle();
    }
}
