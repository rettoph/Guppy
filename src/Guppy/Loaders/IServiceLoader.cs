﻿using Guppy.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        void ConfigureServices(IServiceCollection services);
    }
}
