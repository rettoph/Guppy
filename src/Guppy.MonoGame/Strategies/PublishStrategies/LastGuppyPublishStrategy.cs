using Guppy.Common;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Strategies.PublishStrategies
{
    public sealed class LastGuppyPublishStrategy : PublishStrategy, IDisposable
    {
        private readonly IGuppyProvider _guppies;
        private IScoped<IGuppy>? _last;
        private IBus? _bus;

        public LastGuppyPublishStrategy(IGuppyProvider guppies)
        {
            _guppies = guppies;

            _guppies.OnAdded += this.HandleGuppyAdded;
            _guppies.OnRemoved += this.HandleGuppyRemoved;

            _last = _guppies.All().LastOrDefault();
            _bus = _last?.Scope.ServiceProvider.GetRequiredService<IBus>();
        }

        public void Dispose()
        {
            _guppies.OnAdded -= this.HandleGuppyAdded;
            _guppies.OnRemoved -= this.HandleGuppyRemoved;
        }

        public override void Publish(in IMessage message)
        {
            _bus?.Publish(in message);
        }

        private void HandleGuppyAdded(IGuppyProvider sender, IScoped<IGuppy> args)
        {
            _last = args;
            _bus = _last.Scope.ServiceProvider.GetRequiredService<IBus>();
        }

        private void HandleGuppyRemoved(IGuppyProvider sender, IScoped<IGuppy> args)
        {
            if(_last != args)
            {
                return;
            }

            _last = args;
            _bus = _last.Scope.ServiceProvider.GetRequiredService<IBus>();
        }
    }
}
