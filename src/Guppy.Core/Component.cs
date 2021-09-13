using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Component<TEntity> : Service, IComponent
        where TEntity : class, IEntity
    {
        #region Public Properties
        public TEntity Entity { get; private set; }
        #endregion

        #region IComponent Implementation
        IEntity IComponent.Entity
        {
            get => this.Entity;
            set => this.Entity = value as TEntity;
        }
        #endregion
    }
}
