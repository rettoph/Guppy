using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Guppy.Collections
{
    public class FrameableCollection<TFrameable> : HashSet<TFrameable>
        where TFrameable : IFrameable
    {
        #region Private Attributes
        private IEnumerable<TFrameable> _draws;
        private IEnumerable<TFrameable> _updates;
        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public EventDelegater Events { get; private set; }
        #endregion

        #region Constructors
        public FrameableCollection(IServiceProvider provider)
        {
            this.logger = provider.GetService<ILogger>();
            this.Events = provider.GetService<EventDelegater>();

            this.Events.RegisterDelegate<TFrameable>("added");
            this.Events.RegisterDelegate<TFrameable>("removed");

            this.RemapDraws();
            this.RemapUpdates();
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            foreach (TFrameable frameable in _updates)
                frameable.TryUpdate(gameTime);
        }

        public virtual void TryDraw(GameTime gameTime)
        {
            foreach (TFrameable frameable in _updates)
                frameable.TryDraw(gameTime);
        }
        #endregion

        #region Collection Methods
        public virtual new Boolean Add(TFrameable item)
        {
            if(base.Add(item))
            {
                this.Events.Invoke<TFrameable>("added", item);

                return true;
            }

            return false;
        }

        public virtual new Boolean Remove(TFrameable item)
        {
            if (base.Remove(item))
            {
                this.Events.Invoke<TFrameable>("removed", item);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        protected void RemapDraws()
        {
            _draws = this
                .OrderBy(f => f.DrawOrder);
        }

        protected void RemapUpdates()
        {
            _updates = this
                .OrderBy(f => f.UpdateOrder);
        }
        #endregion
    }
}
