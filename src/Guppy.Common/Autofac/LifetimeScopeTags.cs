using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Autofac
{
    public static class LifetimeScopeTags
    {
        public const string MainScope = nameof(MainScope);
        public const string GuppyScope = nameof(GuppyScope);
    }
}
