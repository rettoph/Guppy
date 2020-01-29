using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    /// <summary>
    /// Represents a string that is dynamically loaded
    /// via the StringLoader...
    /// </summary>
    public sealed class LoadedString
    {
        private String _key;
        private String _value;
        private StringLoader _strings;

        public LoadedString(StringLoader strings)
        {
            _strings = strings;
        }
        public void Set(String key)
        {
            _key = key;
        }

        public String Get()
        {
            if (_key == default(String))
                return default(String);
            else if (!_strings.ContainsKey(_key))
                return _key;

            return _strings[_key];
        }

        public static implicit operator String(LoadedString ls)
        {
            return ls.Get();
        }
    }
}
