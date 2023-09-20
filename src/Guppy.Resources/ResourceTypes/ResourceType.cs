using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Guppy.Resources.ResourceTypes
{
    public abstract class ResourceType<T> : IResourceType
        where T : notnull
    {
        public Type Type => typeof(T);

        public virtual string Name => this.Type.Name;

        public bool TryResolve(ResourcePack pack, string resourceName, string localization, string input)
        {
            Resource<T> resource = Resource.Get<T>(resourceName);

            if (this.TryResolve(resource, input, pack.RootDirectory, out T value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected abstract bool TryResolve(Resource<T> resource, string input, string root, out T value);
    }
}
