﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Providers
{
    internal sealed class CursorProvider : ICursorProvider
    {
        private IList<ICursor> _cursors;

        public CursorProvider()
        {
            _cursors = new List<ICursor>();

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
