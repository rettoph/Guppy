using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input.Providers
{
    public interface ICursorProvider
    {
        ICursor Get(Guid id);
        void Add(ICursor cursor);
        IEnumerable<ICursor> All();
    }
}
