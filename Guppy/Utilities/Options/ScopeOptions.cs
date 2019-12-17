using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Options
{
    internal sealed class ScopeOptions : IDisposable
    {
        private Dictionary<Type, Object> _instances;

        public ScopeOptions()
        {
            _instances = new Dictionary<Type, Object>();
        }

        public void Dispose()
        {
            _instances.Clear();
        }

        public T Get<T>()
        {
            return (T)this.Get(typeof(T));
        }
        public Object Get(Type type)
        {
            if (_instances.ContainsKey(type))
                return _instances[type];
            return
                null;
        }

        public void Set<T>(T value)
        {
            this.Set(typeof(T), value);
        }
        public void Set(Type type, Object value)
        {
            ExceptionHelper.ValidateAssignableFrom(type, value.GetType());

            if (_instances.ContainsKey(type))
                throw new Exception($"Unable to set scoped {type.Name} instance to {type.Name}<{value.GetType().Name}>, value already defined.");

            _instances[type] = value;
        }
    }
}
