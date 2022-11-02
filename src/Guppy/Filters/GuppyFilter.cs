﻿using Guppy.Common;
using Guppy.Common.Filters;
using Guppy.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Filters
{
    public class GuppyFilter : SimpleFilter
    {
        public readonly Type GuppType;

        public GuppyFilter(Type implementationType, Type guppyType) : base(implementationType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppType = guppyType;
        }

        public override bool Invoke(IServiceProvider provider, Type implementationType)
        {
            var guppy = provider.GetRequiredService<Faceted<IGuppy>>();

            if (guppy.Type is null)
            {
                return false;
            }

            var result = this.GuppType.IsAssignableFrom(guppy.Type);

            return result;
        }
    }

    public class GuppyFilter<TImplementation, TGuppy> : GuppyFilter
        where TGuppy : IGuppy
    {
        public GuppyFilter() : base(typeof(TImplementation), typeof(TGuppy))
        {
        }
    }
}
