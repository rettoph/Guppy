using Guppy.Enums;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Service, IEntity
    {
        #region Private Fields
        private IComponent[] _components;
        #endregion

        #region IEntity Implementation
        public IComponent[] Components
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
