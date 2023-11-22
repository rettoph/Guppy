using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public abstract class GuppyComponent : IGuppyComponent
    {
        public virtual void Initialize(IGuppy guppy)
        {
        }
    }
}
