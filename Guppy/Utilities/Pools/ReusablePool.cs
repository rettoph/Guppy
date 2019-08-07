using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class ReusablePool<TReusable> : InitializablePool<TReusable>
        where TReusable : class, IReusable
    {
        public ReusablePool(Type targetType = null) : base(targetType)
        {
        }

        protected override TReusable Create(IServiceProvider provider)
        {
            var child = base.Create(provider);

            // Auto instantiate the child
            child.TryCreate(provider);

            return child;
        }

        public override TReusable Pull(IServiceProvider provider, Action<TReusable> setup = null)
        {
            var child =  base.Pull(provider, setup);

            // Ensure that the child is returned to the pool on dispose
            child.Events.AddDelegate<DateTime>("disposing", this.HandleChildDisposed);

            return child;
        }

        #region Event Handlers
        /// <summary>
        /// Auto return the guppy child to the pool when it is
        /// disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void HandleChildDisposed(object sender, DateTime arg)
        {
            this.Put(sender as TReusable);
        }
        #endregion
    }
}
