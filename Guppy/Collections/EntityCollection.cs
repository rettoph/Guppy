using Guppy.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    class EntityCollection : ReusableCollection<Entity>
    {
        #region Private Fields
        private IServiceProvider _provider;
        #endregion

        #region Constructors
        public EntityCollection(IServiceProvider provider) : base(provider)
        {
            _provider = provider;
        }
        #endregion

        #region Helper Methods
        public TEntity Build<TEntity>(String handle, Action<TEntity> setup = null)
        {
            return _provider.GetPooledService<TEntity>((e) =>
            {
                setup?.Invoke(e);
            });
        }
        #endregion
    }
}
