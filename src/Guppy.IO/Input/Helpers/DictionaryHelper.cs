using Guppy.IO.Input.Delegates;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Input.Helpers
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Construct a new readonly dictionary that contains delegates for every
        /// possible value and populate with defaults.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> BuildEnumDictionary<TKey, TValue>()
            where TKey : Enum
        {
            return ((TKey[])Enum.GetValues(typeof(TKey))).ToDictionary(
                keySelector: k => k,
                elementSelector: k => default(TValue));
        }
    }
}
