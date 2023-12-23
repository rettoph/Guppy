namespace Guppy.Game.Input.Providers
{
    internal sealed class CursorProvider : ICursorProvider
    {
        private IList<ICursor> _cursors;

        public CursorProvider()
        {
            _cursors = new List<ICursor>();

            this.Add(new Cursor());
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
