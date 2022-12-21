using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection.Interfaces
{
    public interface IServiceCollectionManager
    {
        int Order => 0;

        IServiceConfiguration AddService(Type serviceType);

        IServiceConfiguration GetService(Type serviceType, Func<IServiceConfiguration, bool> predicate);

        IServiceConfiguration GetService(Type serviceType);

        void Refresh(IServiceCollection services);
    }
}
