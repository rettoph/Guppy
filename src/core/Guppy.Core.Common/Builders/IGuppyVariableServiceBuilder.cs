using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common.Builders
{
    public interface IGuppyVariableServiceBuilder<TSelf, TVariable, TService> : IGuppyVariableProvider<TVariable>
        where TSelf : IGuppyVariableServiceBuilder<TSelf, TVariable, TService>
        where TVariable : IGuppyVariable
        where TService : IGuppyVariableService<TVariable>
    {
        TSelf Add(TVariable variable);
        IEnumerable<TVariable> GetAll();

        TService Build();
    }
}
