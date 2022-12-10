﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public interface IServiceDescriptorProvider
    {
        IEnumerable<ServiceDescriptor> GetDescriptors();
    }
}
