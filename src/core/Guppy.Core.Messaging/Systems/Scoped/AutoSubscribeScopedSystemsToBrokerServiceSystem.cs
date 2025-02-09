using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging.Systems.Global
{
    public class AutoSubscribeScopedSystemsToBrokerServiceSystem(IMessageBus messageBus, Lazy<IScopedSystemService> scopedSystemService) : IScopedSystem, IDisposable
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly Lazy<IScopedSystemService> _scopedSystemService = scopedSystemService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize(IGuppyScope scope)
        {
            this._messageBus.Subscribe(this._scopedSystemService.Value);
        }

        public void Dispose()
        {
            this._messageBus.Unsubscribe(this._scopedSystemService.Value);
        }
    }
}
