namespace Guppy.Game.Input.Providers
{
    public interface ICursorProvider
    {
        ICursor Get(Guid id);
        void Add(ICursor cursor);
        IEnumerable<ICursor> All();
    }
}
