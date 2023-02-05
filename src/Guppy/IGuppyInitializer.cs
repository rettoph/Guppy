using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public interface IGuppyInitializer
    {
        void Initialize(GuppyEngine engine);
    }
}
