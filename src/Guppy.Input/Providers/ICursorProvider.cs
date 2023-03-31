using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Providers
{
    public interface ICursorProvider
    {
        void Initialize(IList<ICursor> cursors);
        IEnumerable<IMessage> Update();
    }
}
