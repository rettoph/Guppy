﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Common.Filters;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    public class GuppyFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly Type GuppyType;

        public GuppyFilterAttribute(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppyType = guppyType;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            configuration.Services.AddFilter(new ServiceFilter<Type>(classType, this.GuppyType));
        }
    }

    public class GuppyFilterAttribute<TGuppy> : GuppyFilterAttribute
    {
        public GuppyFilterAttribute() : base(typeof(TGuppy)) { }
    }
}
