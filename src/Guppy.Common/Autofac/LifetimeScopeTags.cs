using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Autofac
{
    public static class LifetimeScopeTags
    {
        public static readonly object MainScope = nameof(MainScope);
        public static readonly object GuppyScope = nameof(GuppyScope);
    }
}
