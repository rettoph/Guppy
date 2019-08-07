using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    /// <summary>
    /// Used to retain data between scopes,
    /// When a new scope is created, the scope
    /// data will be copied
    /// </summary>
    public class ScopeConfiguration
    {
        private Dictionary<String, Object> _table;

        public ScopeConfiguration()
        {
            _table = new Dictionary<String, Object>();
        }

        public T Get<T>(String handle)
        {
            return (T)this.Get(handle);
        }
        public Object Get(String handle)
        {
            if (_table.ContainsKey(handle))
                return _table[handle];

            return null;
        }

        public Object Set(String handle, Object value)
        {
            _table[handle] = value;
            return value;
        }

        public void Copy(ScopeConfiguration scope)
        {
            // Copy the scope table
            _table = new Dictionary<String, Object>(scope._table);
        }
    }
}
