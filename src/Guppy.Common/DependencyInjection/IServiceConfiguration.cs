using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public interface IServiceConfiguration : IServiceCollectionManager
    {
        Type ServiceType { get; }

        Type? ImplementationType { get; }

        Type Type { get; }

        ServiceLifetime? Lifetime { get; }

        Func<IServiceProvider, object>? Factory { get; }

        IReadOnlyDictionary<Type, AliasDescriptor> Aliases { get; }

        IServiceConfiguration SetImplementationType(Type? implementationType);

        IServiceConfiguration SetImplementationType<TImplementation>()
            where TImplementation: class;

        IServiceConfiguration SetLifetime(ServiceLifetime? lifetime);

        IServiceConfiguration SetInstance(object? instance);

        IServiceConfiguration SetFactory(Func<IServiceProvider, object>? factory);

        IServiceConfiguration AddAlias(Type alias, AliasType type = AliasType.Filtered);

        IServiceConfiguration AddAlias<TAlias>(AliasType type = AliasType.Filtered);

        IServiceConfiguration AddAliases(AliasType type = AliasType.Filtered, params Type[] aliases);

        IServiceConfiguration AddInterfaceAliases(AliasType type = AliasType.Filtered);

        IEnumerable<AliasDescriptor> GetAliasDescriptors(AliasType type);
    }
}
