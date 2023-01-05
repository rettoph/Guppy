﻿using Guppy.Common.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Filters
{
    public abstract class SimpleFilter : IServiceFilter
    {
        public readonly Type Type;

        protected SimpleFilter(Type type)
        {
            this.Type = type;
        }

        public virtual void Initialize(IServiceProvider provier)
        {
            //
        }

        public virtual bool AppliesTo(Type type)
        {
            var result = this.Type.IsAssignableFrom(type);

            return result;
        }

        public abstract bool Invoke(IServiceProvider provider, object service);
    }
}