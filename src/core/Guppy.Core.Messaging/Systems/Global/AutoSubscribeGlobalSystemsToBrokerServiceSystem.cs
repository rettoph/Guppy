using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Systems.Global
{
    public class AutoSubscribeGlobalSystemsToBrokerServiceSystem(
        IBrokerService brokerService,
        Lazy<IGlobalSystemService> globalSystemService
    ) : IGlobalSystem,
        IInitializeSystem<object>,
        IDeinitializeSystem<object>
    {
        private readonly IBrokerService _brokerService = brokerService;
        private readonly Lazy<IGlobalSystemService> _globalSystemService = globalSystemService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize(object obj)
        {
            this._brokerService.AddSubscribers<IGlobalSystem>();
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.PreInitialize)]
        public void Deinitialize(object obj)
        {
            this._brokerService.RemoveSubscribers<IGlobalSystem>();
        }
    }
}
