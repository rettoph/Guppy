using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal sealed class ContentResource<T> : Resource<T>
    {
        public ContentResource(T value, string name, string? source, IResourcePack pack) : base(value, name, source, pack)
        {
        }
    }
}
