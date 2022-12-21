using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection.Interfaces
{
    public interface IServiceConfiguration
    {
        Type ServiceType { get; }

        Type? ImplementationType { get; }

        Type Type { get; }

        ServiceLifetime? Lifetime { get; }

        Func<IServiceProvider, object>? Factory { get; }

        AliasesConfiguration Aliases { get; }

        IServiceConfiguration SetImplementationType(Type? implementationType);

        IServiceConfiguration SetImplementationType<TImplementation>()
            where TImplementation : class;

        IServiceConfiguration SetLifetime(ServiceLifetime? lifetime);

        IServiceConfiguration SetInstance(object? instance);

        IServiceConfiguration SetFactory(Func<IServiceProvider, object>? factory);

        IServiceConfiguration AddAlias(Type alias, Action<AliasConfiguration>? configure = null);

        IServiceConfiguration AddAlias<TAlias>(Action<AliasConfiguration>? configure = null);

        IServiceConfiguration AddAliases(Action<AliasConfiguration>? configure = null, params Type[] aliases);

        IServiceConfiguration AddInterfaceAliases(Action<AliasConfiguration>? configure = null);
        internal void Refresh(IServiceCollection services);
    }
}
