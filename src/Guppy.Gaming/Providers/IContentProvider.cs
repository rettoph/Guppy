using Guppy.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Providers
{
    public interface IContentProvider : IResourceProvider<IContent>
    {
        Content<T> Get<T>(string key);
        bool TryGet<T>(string key, [MaybeNullWhen(false)] out Content<T> content);
    }
}
