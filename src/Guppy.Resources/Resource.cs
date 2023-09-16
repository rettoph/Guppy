using Guppy.Common.Collections;
using Standart.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Guppy.Resources
{
    public unsafe abstract class Resource
    {
        private static DoubleDictionary<Guid, string, Resource> _resources = new DoubleDictionary<Guid, string, Resource>();

        public readonly Guid Id;
        public readonly string Name;
        public readonly Type Type;

        internal Resource(string name, Type type)
        {
            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = name;

            _resources.TryAdd(this.Id, this.Name, this);
            Type = type;
        }

        public static Resource Get(Guid id)
        {
            return _resources[id];
        }
        
        public static Resource<T> Get<T>(string name)
            where T : notnull
        {
            if(_resources.TryGet(name, out Resource? resource))
            {
                return (Resource<T>)resource;
            }

            return new Resource<T>(name);
        }
    }

    public sealed class Resource<T> : Resource
        where T : notnull
    {
        internal Resource(string name) : base(name, typeof(T))
        {
        }
    }
}
