using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;

namespace Guppy.Game.Input.Services
{
    internal sealed class CursorService : ICursorService
    {
        private readonly IList<ICursor> _cursors;

        public CursorService()
        {
            this._cursors = [];

            this.Add(new MouseCursor());
        }

        public void Add(ICursor cursor) => this._cursors.Add(cursor);

        public IEnumerable<ICursor> All() => this._cursors;

        public ICursor Get(Guid id) => this._cursors.First(c => c.Id == id);
    }
}