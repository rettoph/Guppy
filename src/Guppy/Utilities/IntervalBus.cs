using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Utilities
{
    public class IntervalBus : Service
    {
        #region Private Fields
        private IntervalInvoker _intervals;
        private Dictionary<Double, IntervalQueue> _queues;
        #endregion

        #region Public Properties
        public IntervalQueue this[Double milliseconds]
        {
            get
            {
                if (!_queues.TryGetValue(milliseconds, out IntervalQueue queue))
                {
                    queue = new IntervalQueue(_intervals[milliseconds]);
                    _queues.Add(milliseconds, queue);
                }

                return queue;
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _intervals);

            _queues = new Dictionary<Double, IntervalQueue>();
        }
        #endregion
    }
}
