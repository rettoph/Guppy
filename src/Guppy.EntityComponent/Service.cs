using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.Enums;

namespace Guppy.EntityComponent
{
    public abstract class Service : IService, IDisposable
    {
        #region Private Fields
        private ServiceConfiguration _configuration;
        private Guid _id;
        private ServiceStatus _status = ServiceStatus.Initializing;
        #endregion

        #region Public Properties
        public ServiceConfiguration ServiceConfiguration
        {
            get => _configuration;
            set
            {
                if (this.Status == ServiceStatus.Ready)
                    throw new InvalidOperationException("Unable to update Configuration after initialization.");

                _configuration = value;
            }
        }
        public virtual Guid Id { get; protected set; } = Guid.NewGuid();

        public ServiceStatus Status
        {
            get => _status;
            set => this.OnStatusChanged.InvokeIf(value != _status, this, ref _status, value);
        }
        #endregion

        #region Events
        public event OnChangedEventDelegate<IService, ServiceStatus> OnStatusChanged;
        #endregion

        #region Lifecycle Methods
        void IService.TryPreInitialize(ServiceProvider provider)
        {
            this.PreInitialize(provider);
        }

        void IService.TryInitialize(ServiceProvider provider)
        {
            this.Initialize(provider);
        }

        void IService.TryPostInitialize(ServiceProvider provider)
        {
            this.PostInitialize(provider);
            this.Status = ServiceStatus.Ready;
        }

        public void Dispose()
        {
            if(this.GetType().Name == "Ship")
            {

            }

            if(this.Status != ServiceStatus.Ready)
            {
                return;
            }

            this.Status = ServiceStatus.Uninitializing;

            this.PreUninitialize();
            this.Uninitialize();
            this.PostUninitialize();

            this.Status = ServiceStatus.Disposed;
        }

        protected virtual void PreInitialize(ServiceProvider provider)
        {
            //
        }

        protected virtual void Initialize(ServiceProvider provider)
        {
            //
        }

        protected virtual void PostInitialize(ServiceProvider provider)
        {
            //
        }

        protected virtual void PreUninitialize()
        {
            //
        }

        protected virtual void Uninitialize()
        {
            //
        }

        protected virtual void PostUninitialize()
        {
            //
        }
        #endregion
    }
}
