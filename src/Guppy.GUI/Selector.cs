using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public sealed class Selector
    {
        public readonly Type Type;
        public readonly string[] Names;
        public readonly Selector? Parent;
    }
}
