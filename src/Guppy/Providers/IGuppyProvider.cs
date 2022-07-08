using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IGuppyProvider : IEnumerable<IGuppy>
    {
        TGuppy Create<TGuppy>()
            where TGuppy : class, IGuppy;

        IEnumerable<TGuppy> Get<TGuppy>()
            where TGuppy : class, IGuppy;
    }
}
