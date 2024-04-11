namespace Guppy.Game.Input.Services
{
    public interface ICursorService
    {
        ICursor Get(Guid id);
        void Add(ICursor cursor);
        IEnumerable<ICursor> All();
    }
}
