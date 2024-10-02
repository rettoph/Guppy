using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;

namespace Guppy.Game.Input.Services
{
    internal sealed class CursorService : ICursorService
    {
        private readonly IList<ICursor> _cursors;

        public CursorService()
        {
            _cursors = [];

            this.Add(new MouseCursor());
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
