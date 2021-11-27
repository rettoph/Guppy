using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DynamicIdConfigurationBuilder<T>
        where T : DynamicIdConfigurationBuilder<T>
    {
        #region Public Properties
        public UInt16? Id { get; private set; }
        #endregion

        #region SetId Methods
        public T SetId(UInt16? id)
        {
            this.Id = id;

            return this as T;
        }
        #endregion
    }
}
