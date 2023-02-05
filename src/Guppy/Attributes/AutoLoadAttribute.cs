using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoLoadAttribute : Attribute
    {
        public AutoLoadAttribute()
        {
        }
    }
}
