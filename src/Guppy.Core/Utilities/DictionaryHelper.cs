using System;
using System.Collections.Generic;
using System.Linq;

namespace Guppy.Utilities
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Construct a new readonly dictionary that contains values for every
        /// possible enum key and populate with defaults.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> BuildEnumDictionary<TKey, TValue>()
            where TKey : Enum
        {
            return EnumHelper.GetValues<TKey>().ToDictionary(
                keySelector: k => k,
                elementSelector: k => default(TValue));
        }
    }
}
