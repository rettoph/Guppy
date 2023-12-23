namespace Guppy.Network
{
    public interface IRecyclable : IDisposable
    {
        void Recycle();
    }
}
