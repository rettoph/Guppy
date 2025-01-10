namespace Guppy.Game.Input.Common.Services
{
    public interface ICursorService
    {
        ICursor Get(Guid id);
        void Add(ICursor cursor);
        IEnumerable<ICursor> All();
    }
}