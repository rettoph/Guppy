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

        internal Resource(string name)
        {
            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = name;

            _resources.TryAdd(this.Id, this.Name, this);
        }

        public static Resource Get(Guid id)
        {
            return _resources[id];
        }
    }

    public sealed class Resource<T> : Resource
        where T : notnull
    {
        public Resource(string name) : base(name)
        {
        }
    }
}
