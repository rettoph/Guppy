using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IResourceProvider<T> : IEnumerable<T>
    {
        T this[string key] => this.Get(key);

        bool TryGet(string key, [MaybeNullWhen(false)] out T resource);

        T Get(string key);

        void Import(Dictionary<string, string?> values);

        Dictionary<string, string?> Export();
    }
}
