using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public abstract class ResourceProvider<T> : IResourceProvider<T>
        where T : IResource
    {
        public virtual T this[string key] => this.Get(key);

        public abstract bool TryGet(string key, [MaybeNullWhen(false)] out T resource);

        public virtual T Get(string key)
        {
            if (this.TryGet(key, out T? resource))
            {
                return resource;
            }

            throw new ArgumentOutOfRangeException();
        }

        public virtual void Import(Dictionary<string, string?> values)
        {
            foreach((string key, string? value) in values)
            {
                if(this.TryGet(key, out T? resource))
                {
                    resource.Import(value);
                }
            }
        }

        public virtual Dictionary<string, string?> Export()
        {
            Dictionary<string, string?> values = new Dictionary<string, string?>();

            foreach(IResource resource in this)
            {
                values.Add(resource.Key, resource.Export());
            }

            return values;
        }

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
