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
    internal sealed class UpdateTimersComponent<TSubscribableFrameable> : Component<TSubscribableFrameable>
        where TSubscribableFrameable : Frameable, ISubscribableFrameable
    {
        private TSubscribableFrameable _subscribable;
        private ITimerProvider _timers;

        public UpdateTimersComponent(ITimerProvider timers)
        {
            _timers = timers;
            _subscribable = null!;
        }

        protected override void Initialize(TSubscribableFrameable entity)
        {
            _subscribable = entity;
            _subscribable.OnUpdate += this.Update;
        }

        protected override void Uninitilaize()
        {
            _subscribable.OnUpdate -= this.Update;
        }

        private void Update(GameTime gameTime)
        {
            _timers.Update(gameTime);
        }
    }
}
