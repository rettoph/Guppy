using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface ISequenceable<TSequence>
        where TSequence : unmanaged, Enum
    {
        public TSequence? Sequence => null;
    }
}
