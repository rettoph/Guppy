using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface IResourcePack
    {
        string Name { get; }
        string Path { get; }

        bool TryGet<T>(string name, [MaybeNullWhen(false)] out IResource<T> value);

        IEnumerable<string> SearchForFiles(string pattern);
    }
}
