﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceScopeFactory : IServiceScopeFactory
    {
        private ServiceProvider _parent;

        internal ServiceScopeFactory(ServiceProvider parent)
            => _parent = parent;

        public IServiceScope CreateScope()
            => new ServiceScope(_parent);
    }
}
