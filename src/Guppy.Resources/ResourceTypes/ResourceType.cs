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

        public bool TryResolve(ResourcePack pack, string resourceName, string localization, string input)
        {
            Resource<T> resource = Resource.Get<T>(resourceName);
            Configure(pack);

            if (this.TryResolve(resource, input, out T value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected virtual void Configure(ResourcePack pack)
        {

        }

        protected abstract bool TryResolve(Resource<T> resource, string input, out T value);
    }
}
