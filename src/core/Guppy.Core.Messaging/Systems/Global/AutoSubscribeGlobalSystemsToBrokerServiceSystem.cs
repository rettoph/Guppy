using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging.Systems.Global
{
    public class AutoSubscribeGlobalSystemsToBrokerServiceSystem(
        IMessageBus brokerService,
        Lazy<IGlobalSystemService> globalSystemService
    ) : IGlobalSystem,
        IInitializeSystem,
        IDeinitializeSystem
    {
        private readonly IMessageBus _messageBus = brokerService;
        private readonly Lazy<IGlobalSystemService> _globalSystemService = globalSystemService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize()
        {
            this._messageBus.Subscribe(this._globalSystemService.Value);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.PreInitialize)]
        public void Deinitialize()
        {
            this._messageBus.Unsubscribe(this._globalSystemService.Value);
        }
    }
}
