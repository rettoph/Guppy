using System;
using System.Collections;

namespace Guppy.Resources
{
    internal class RawResourceValues : IEnumerable<string>
    {
        private string[] _values;

        internal RawResourceValues(params string[] values)
        {
            _values = values;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}
