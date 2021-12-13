using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using log4net;
using Guppy.Extensions.log4net;
using Guppy.Extensions.DependencyInjection;
using DotNetUtils.DependencyInjection;

namespace Guppy
{
    public abstract class Service : IService, IDisposable
    {
        #region Private Fields
        private ServiceConfiguration<GuppyServiceProvider> _configuration;
        private Guid _id;
        private ServiceStatus _status;
        #endregion

        #region Protected Properties
        protected internal ILog log { get; private set; }
        #endregion

        #region Public Properties
        public ServiceConfiguration<GuppyServiceProvider> ServiceConfiguration
        {
            get => _configuration;
            set
            {
                if (this.Status == ServiceStatus.Ready)
                    throw new InvalidOperationException("Unable to update Configuration after initialization.");

                _configuration = value;
            }
        }
        public virtual Guid Id
        {
            get => _id;
            set
            {
                if (this.Status == ServiceStatus.Ready)
                    throw new InvalidOperationException("Unable to update Id after initialization.");

                _id = value;
            }
        }

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
        void IService.TryPreCreate(GuppyServiceProvider provider)
        {
            this.log = provider.GetService<ILog>();

            if (this.ValidateStatus(ServiceStatus.NotCreated))
            {
                this.Status = ServiceStatus.PreCreating;
                this.PreCreate(provider);
            }
        }
        void IService.TryCreate(GuppyServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.PreCreating))
            {
                this.Status = ServiceStatus.Creating;
                this.Create(provider);
            }
        }

        void IService.TryPostCreate(GuppyServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.Creating))
            {
                this.Status = ServiceStatus.PostCreating;
                this.PostCreate(provider);

                this.Status = ServiceStatus.NotInitialized;
            }
        }

        void IService.TryPreInitialize(GuppyServiceProvider provider)
        {
            if (this.ValidateStatus(ServiceStatus.NotInitialized))
            {
                this.Status = ServiceStatus.PreInitializing;
                this.PreInitialize(provider);
            }
        }

        void IService.TryInitialize(GuppyServiceProvider provider)
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

        void IService.TryPostInitialize(GuppyServiceProvider provider)
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

                this.log = default;
            }
        }

        protected virtual void PreCreate(GuppyServiceProvider provider)
        {
            //
        }

        protected virtual void Create(GuppyServiceProvider provider)
        {
            //
        }

        protected virtual void PostCreate(GuppyServiceProvider provider)
        {
            //
        }

        protected virtual void PreInitialize(GuppyServiceProvider provider)
        {
            this.Id = Guid.NewGuid();
        }

        protected virtual void Initialize(GuppyServiceProvider provider)
        {
            //
        }

        protected virtual void PostInitialize(GuppyServiceProvider provider)
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
