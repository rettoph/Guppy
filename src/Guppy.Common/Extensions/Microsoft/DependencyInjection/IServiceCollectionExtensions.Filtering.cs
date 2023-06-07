using Guppy;
using Guppy.Common;
using Guppy.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFilter(
            this IServiceCollection services,
            IServiceFilter filter)
        {
            services.AddSingleton(filter);

            return services;
        }
    }
}
