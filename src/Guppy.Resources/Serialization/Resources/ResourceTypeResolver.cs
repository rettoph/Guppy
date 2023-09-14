using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Resources
{
    public abstract class ResourceTypeResolver<T> : IResourceTypeResolver
        where T : notnull
    {
        public Type Type => typeof(T);

        public bool TryResolve(ResourcePack pack, Resource resource, string localization, string input)
        {
            this.Configure(pack);

            if(resource is Resource<T> casted && this.TryResolve(casted, input, out T value))
            {
                pack.Add(casted, localization, value);
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
