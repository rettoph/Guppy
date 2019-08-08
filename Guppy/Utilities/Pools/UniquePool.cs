using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class UniquePool<TUnique> : ServicePool<TUnique>
        where TUnique : class, IUnique
    {
        public UniquePool(Type targetType = null) : base(targetType)
        {
        }

        protected override TUnique Create(IServiceProvider provider)
        {
            var child = base.Create(provider);

            // Auto instantiate the child
            child.TryCreate(provider);

            return child;
        }

        public override TUnique Pull(IServiceProvider provider, Action<TUnique> setup = null)
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
            this.Put(sender as TUnique);
        }
        #endregion
    }
}
