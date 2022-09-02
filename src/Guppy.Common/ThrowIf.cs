using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public static class ThrowIf
    {
        public static class Type
        {
            public static void IsNotAssignableFrom(System.Type parent, System.Type child)
            {
                if (!parent.IsAssignableFrom(child))
                {
                    throw new ArgumentException($"'{parent.FullName}' is not assignable from '{child.FullName}'.");
                }
            }

            public static void IsNotAssignableFrom<TParent>(System.Type child)
            {
                ThrowIf.Type.IsNotAssignableFrom(typeof(TParent), child);
            }
        }
    }
}
