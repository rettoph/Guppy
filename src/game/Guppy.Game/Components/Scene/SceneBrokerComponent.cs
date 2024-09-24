﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    [AutoLoad]
    internal class SceneBrokerComponent : ISceneComponent
    {
        private readonly IBrokerService _brokers;

        public SceneBrokerComponent(IBrokerService brokers)
        {
            _brokers = brokers;
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            _brokers.AddSubscribers<ISceneComponent>();
        }

        public void Dispose()
        {
            _brokers.RemoveSubscribers<ISceneComponent>();
        }
    }
}
