﻿using Guppy.Attributes;
using Guppy.Common.Services;

namespace Guppy.Components.Guppy
{
    [AutoLoad]
    internal class GuppyBulkSubscriptionComponent : GuppyComponent
    {
        private readonly IBulkSubscriptionService _subscriptions;
        private IGuppyComponent[] _components;

        public GuppyBulkSubscriptionComponent(IBulkSubscriptionService subscriptions)
        {
            _subscriptions = subscriptions;
            _components = Array.Empty<IGuppyComponent>();
        }

        public override void Initialize(IGuppy guppy)
        {
            base.Initialize(guppy);

            _components = guppy.Components;

            _subscriptions.Subscribe(_components);
        }

        public void Dispose()
        {
            _subscriptions.Unsubscribe(_components);
        }
    }
}
