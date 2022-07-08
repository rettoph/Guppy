using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IResourcePackProvider : IEnumerable<IResourcePack>
    {
        bool TryGet(string name, [MaybeNullWhen(false)] IResourcePack pack);
    }
}
