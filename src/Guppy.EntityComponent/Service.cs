﻿using System;
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
        private ServiceStatus _status;
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

#if DEBUG
        public UInt32 HistoricInitializedCount { get; private set; } = 0;
#endif

#endregion

        #region Events
        public event OnChangedEventDelegate<IService, ServiceStatus> OnStatusChanged;
        #endregion

        #region Lifecycle Methods
        void IService.TryPreCreate(ServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.NotCreated))
            {
                this.Status = ServiceStatus.PreCreating;
                this.PreCreate(provider);
            }
        }
        void IService.TryCreate(ServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.PreCreating))
            {
                this.Status = ServiceStatus.Creating;
                this.Create(provider);
            }
        }

        void IService.TryPostCreate(ServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.Creating))
            {
                this.Status = ServiceStatus.PostCreating;
                this.PostCreate(provider);

                this.Status = ServiceStatus.NotInitialized;
            }
        }

        void IService.TryPreInitialize(ServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.NotInitialized))
            {
                this.Status = ServiceStatus.PreInitializing;
                this.PreInitialize(provider);
            }
        }

        void IService.TryInitialize(ServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.PreInitializing))
            {
#if DEBUG
                this.HistoricInitializedCount++;
#endif
                this.Status = ServiceStatus.Initializing;
                this.Initialize(provider);
            }
        }

        void IService.TryPostInitialize(ServiceProvider provider)
        {
            if(this.ValidateStatus(ServiceStatus.Initializing))
            {
                this.Status = ServiceStatus.PostInitializing;
                this.PostInitialize(provider);

                this.Status = ServiceStatus.Ready;
            }
        }

        public void TryRelease()
        {
            if(this.ValidateStatus(status: ServiceStatus.Ready, required: false))
            {
                this.Status = ServiceStatus.PreReleasing;
                this.PreRelease();

                this.Status = ServiceStatus.Releasing;
                this.Release();

                this.Status = ServiceStatus.PostReleasing;
                this.PostRelease();

                this.Status = ServiceStatus.NotInitialized;
            }
        }

        public void TryDispose()
        {
            this.TryRelease();

            if (this.ValidateStatus(status: ServiceStatus.NotInitialized, required: false))
            {
                this.Status = ServiceStatus.PreDisposing;
                this.PreDispose();

                this.Status = ServiceStatus.Disposing;
                this.Dispose();

                this.Status = ServiceStatus.PostDisposing;
                this.PostDispose();

                this.Status = ServiceStatus.NotCreated;
            }
        }

        protected virtual void PreCreate(ServiceProvider provider)
        {
            //
        }

        protected virtual void Create(ServiceProvider provider)
        {
            //
        }

        protected virtual void PostCreate(ServiceProvider provider)
        {
            //
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

        protected virtual void PreRelease()
        {
            //
        }

        protected virtual void Release()
        {
            //
        }

        protected virtual void PostRelease()
        {
            //
        }

        protected virtual void PreDispose()
        {
            //
        }

        protected virtual void Dispose()
        {
            //
        }

        protected virtual void PostDispose()
        {
            //
        }
        #endregion

        #region IDisposable Implementation
        void IDisposable.Dispose()
            => this.TryRelease();
        #endregion

        #region Helper Methods
        private Boolean ValidateStatus(ServiceStatus status, Boolean required = true)
        {
            if (this.Status != status)
                return required ? throw new Exception($"Invalid Status, expected '{status}' but got '{this.Status}'.") : false;

            return true;
        }
        #endregion
    }
}