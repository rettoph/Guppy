using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Systems.Global
{
    public class AutoSubscribeScopedSystemsToBrokerServiceSystem(IBrokerService brokerService) : IScopedSystem, IDisposable
    {
        private readonly IBrokerService _brokerService = brokerService;

        [SequenceGroup<InitializeSystemSequenceGroupEnum>(InitializeSystemSequenceGroupEnum.PreInitialize)]
        public void Initialize(IGuppyScope scope)
        {
            this._brokerService.AddSubscribers<IScopedSystem>();
        }

        public void Dispose()
        {
            this._brokerService.RemoveSubscribers<IScopedSystem>();
        }
    }
}
