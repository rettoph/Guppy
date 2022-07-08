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
        T Get<T>(string name);
        bool TryGet<T>(string name, [MaybeNullWhen(false)] T resource);
    }
}
