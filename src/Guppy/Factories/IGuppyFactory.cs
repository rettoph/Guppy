using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Factories
{
    public interface IGuppyFactory
    {
        void Build(GuppyEngine guppy);
    }
}
