using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class IResourceExtensions
    {
        public static Dictionary<string, T> ToDictionary<T>(this IEnumerable<T> resources)
            where T : IResource
        {
            return resources.ToDictionary(r => r.Key);
        }
    }
}
