using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IResourceProvider
    {
        IResource<T> Get<T>(string name);
        bool TryGet<T>(string name, [MaybeNullWhen(false)] IResource<T> resource);
        IEnumerable<T> GetAll<T>()
            where T : IResource;
    }
}
