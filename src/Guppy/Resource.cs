using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public abstract class Resource<T> : IResource
    {
        public readonly string Key;
        public readonly T DefaultValue;
        public T Value;

        string IResource.Key => this.Key;

        protected Resource(string key, T defaultValue)
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
            this.Value = defaultValue;
        }

        public object? GetDefaultValue()
        {
            return this.DefaultValue;
        }

        public object? GetValue()
        {
            return this.Value;
        }

        public bool TrySetValue(object value)
        {
            if(value is T casted)
            {
                this.Value = casted;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.Value = this.DefaultValue;
        }

        public abstract string? Export();

        public abstract void Import(string? value);

        public static implicit operator T(Resource<T> resource)
        {
            return resource.Value;
        }
    }
}
