using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Service, IEntity
    {
        #region Private Fields
        private ComponentManager _components;
        #endregion

        #region IEntity Implementation
        public ComponentManager Components
        {
            get => _components;
            set
            {
                if (this.Status != ServiceStatus.NotInitialized && this.Status != ServiceStatus.PostReleasing)
                    throw new InvalidOperationException("Unable to update Components after initialization has begun.");

                _components = value;
            }
        }
        #endregion
    }
}
