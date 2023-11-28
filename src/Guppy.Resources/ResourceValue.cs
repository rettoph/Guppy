﻿using Guppy.Common;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal interface IResourceValue
    {
        void ForceUpdate(ResourceProvider resources);
    }

    public sealed class ResourceValue<T> : Ref<T>, IResourceValue
        where T : notnull
    {
        public readonly Resource<T> Resource;

        public ResourceValue(Resource<T> resource) : base(default!)
        {
            this.Resource = resource;
        }

        void IResourceValue.ForceUpdate(ResourceProvider resources)
        {
            this.Value = resources.GetPackValue(this.Resource);
        }
    }
}
