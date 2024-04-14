namespace Guppy.Game.Input.Services
{
    internal sealed class CursorService : ICursorService
    {
        private IList<ICursor> _cursors;

        public CursorService()
        {
            _cursors = new List<ICursor>();

            Add(new Cursor());
        }

        public void Add(ICursor cursor)
        {
            _cursors.Add(cursor);
        }

        public IEnumerable<ICursor> All()
        {
            return _cursors;
        }

        public ICursor Get(Guid id)
        {
            return _cursors.First(c => c.Id == id);
        }
    }
}
