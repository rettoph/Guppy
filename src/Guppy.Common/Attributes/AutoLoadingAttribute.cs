using Guppy.Attributes.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    public abstract class AutoLoadingAttribute : InitializableAttribute
    {
        protected override bool ShouldInitialize(IServiceCollection services, Type classType)
        {
            return classType.HasCustomAttribute<AutoLoadAttribute>() && base.ShouldInitialize(services, classType);
        }
    }
}
