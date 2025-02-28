using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging.Systems.Scoped
{
    public class AutoSubscribeScopedSystemsToBrokerServiceSystem(
        IMessageBus messageBus,
        IScopedSystemService scopedSystemService
    ) : IScopedSystem, IInitializeSystem, IDeinitializeSystem
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IScopedSystemService _scopedSystemService = scopedSystemService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize()
        {
            this._messageBus.SubscribeAll(this._scopedSystemService.GetAll());
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.PreInitialize)]
        public void Deinitialize()
        {
            this._messageBus.UnsubscribeAll(this._scopedSystemService.GetAll());
        }
    }
}
