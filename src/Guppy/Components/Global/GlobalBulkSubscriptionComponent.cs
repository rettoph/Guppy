using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Components.Global
{
    [AutoLoad]
    internal class GlobalBulkSubscriptionComponent : GlobalComponent, IDisposable
    {
        private readonly IBulkSubscriptionService _subscriptions;
        private IGlobalComponent[] _components;

        public GlobalBulkSubscriptionComponent(IBulkSubscriptionService subscriptions)
        {
            _subscriptions = subscriptions;
            _components = Array.Empty<IGlobalComponent>();
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components;

            _subscriptions.Subscribe(_components);
        }

        public void Dispose()
        {
            _subscriptions.Unsubscribe(_components);
        }
    }
}
