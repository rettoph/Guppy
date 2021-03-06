﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    public class Settings
    {
        #region Private Fields
        private Dictionary<String, Object> _values;
        #endregion

        #region Constructor
        public Settings()
        {
            _values = new Dictionary<String, Object>();
        }
        #endregion

        #region Helper Methods
        public void Set(String key, Object value)
        {
            _values[key] = value;
        }
        public void Set<T>(T value)
        {
            _values[typeof(T).FullName] = value;
        }

        public Object Get(String key)
        {
            return _values[key];
        }

        public T Get<T>(String key)
        {
            return (T)this.Get(key);
        }
        public T Get<T>()
        {
            return (T)this.Get(typeof(T).FullName);
        }

        public void Import(Settings settings)
        {
            foreach (KeyValuePair<String, Object> kvp in settings._values)
                _values.Add(kvp.Key, kvp.Value);
        }
        #endregion
    }
}
