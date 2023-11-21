using Guppy.Attributes;
using Guppy.Common;
using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    [Service<IGuppyComponent>(ServiceLifetime.Scoped, true)]
    public interface IGuppyComponent : ISequenceable<InitializeSequence>
    {
        void Initialize(IGuppy guppy);
    }
}
