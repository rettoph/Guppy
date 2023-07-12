using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    public class SequenceAttribute<TSequence> : Attribute
        where TSequence : Enum
    {
        public readonly TSequence Value;

        public SequenceAttribute(TSequence value)
        {
            Value = value;
        }
    }
}
