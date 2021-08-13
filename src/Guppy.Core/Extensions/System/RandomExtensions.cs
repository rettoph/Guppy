using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class RandomExtensions
    {
        public static Single NextSingle(this Random rand, Single min, Single max)
        {
            return (Single)((rand.NextDouble() * (max - min)) + min);
        }

        public static T Next<T>(this Random rand, IEnumerable<T> collection)
        {
            return collection.ElementAt(rand.Next(collection.Count()));
        }
    }
}
