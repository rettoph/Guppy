using Guppy.EntityComponent;
using Guppy.Gaming.Providers;
using Guppy.Gaming.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Components
{
    internal sealed class UpdateTimersComponent<TSubscribableFrameable> : IComponent
        where TSubscribableFrameable : ISubscribableFrameable
    {
        private TSubscribableFrameable _subscribable;
        private ITimerProvider _timers;

        public UpdateTimersComponent(TSubscribableFrameable subscribable, ITimerProvider timers)
        {
            _subscribable = subscribable;
            _timers = timers;

            _subscribable.OnUpdate += this.Update;
        }

        public void Dispose()
        {
            _subscribable.OnUpdate -= this.Update;
        }

        private void Update(GameTime gameTime)
        {
            _timers.Update(gameTime);
        }
    }
}
