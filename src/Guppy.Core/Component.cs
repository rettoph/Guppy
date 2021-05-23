using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Component : Service, IComponent
    {
        #region Private Fields
        public IEntity Entity { get; private set; }
        #endregion

        #region IComponent Implementation
        IEntity IComponent.Entity
        {
            get => this.Entity;
            set => this.SetEntity(value);
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.OnStatus[ServiceStatus.Initializing] += this.HandleEntityInitializing;
            this.Entity.OnStatus[ServiceStatus.Releasing] += this.HandleEntityReleasing;
        }
        #endregion

        #region Helper Methods
        internal virtual void SetEntity(IEntity value)
        {
            if (this.Status == ServiceStatus.Ready)
                throw new InvalidOperationException("Unable to update Entity after initialization has begun.");

            this.Entity = value;
        }
        #endregion

        #region Event Handlers
        protected virtual void HandleEntityInitializing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            this.Entity.OnStatus[ServiceStatus.Initializing] -= this.HandleEntityInitializing;
        }

        protected virtual void HandleEntityReleasing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            this.Entity.OnStatus[ServiceStatus.Releasing] -= this.HandleEntityReleasing;
        }
        #endregion
    }

    public class Component<TEntity> : Component
        where TEntity : class, IEntity
    {
        #region IComponent Implementation
        public new TEntity Entity { get; private set; }
        #endregion

        internal override void SetEntity(IEntity value)
        {
            base.SetEntity(value);

            this.Entity = value as TEntity;
        }
    }
}
