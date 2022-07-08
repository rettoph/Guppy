using Guppy;
using Guppy.Providers;
using Guppy.Resources.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddColorResource(this IServiceCollection services, string name)
        {
            return services.AddResource<Color>(name, null);
        }
    }
}
