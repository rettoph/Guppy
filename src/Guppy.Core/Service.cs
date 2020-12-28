using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Events.Delegates;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy
{
    public abstract class Service : IService, IDisposable
    {
        #region Private Fields
        private ServiceConfiguration _configuration;
        private Guid _id;
        private ServiceStatus _initializationStatus;
        #endregion

        #region Public Attributes
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
            get => _initializationStatus;
            set
            {
                if(value != _initializationStatus)
                {
                    _initializationStatus = value;
                    this.OnStatus[value]?.Invoke(this);
                }
            }
        }
        #endregion

        #region Events
        public Dictionary<ServiceStatus, OnEventDelegate<IService>> OnStatus { get; private set; }
        #endregion

        #region Lifecycle Methods
        void IService.TryPreCreate(ServiceProvider provider)
        {
            this.OnStatus = DictionaryHelper.BuildEnumDictionary<ServiceStatus, OnEventDelegate<IService>>();

            this.ValidateStatus(ServiceStatus.NotCreated);

            this.Status = ServiceStatus.PreCreating;
            this.PreCreate(provider);
        }

        void IService.TryCreate(ServiceProvider provider)
        {
            this.ValidateStatus(ServiceStatus.PreCreating);

            this.Status = ServiceStatus.Creating;
            this.Create(provider);
        }

        void IService.TryPostCreate(ServiceProvider provider)
        {
            this.ValidateStatus(ServiceStatus.Creating);

            this.Status = ServiceStatus.PostCreating;
            this.PostCreate(provider);

            this.Status = ServiceStatus.NotReady;
        }

        void IService.TryPreInitialize(ServiceProvider provider)
        {
            this.ValidateStatus(ServiceStatus.NotReady);

            this.Status = ServiceStatus.PreInitializing;
            this.PreInitialize(provider);
        }

        void IService.TryInitialize(ServiceProvider provider)
        {
            this.ValidateStatus(ServiceStatus.PreInitializing);

            this.Status = ServiceStatus.Initializing;
            this.Initialize(provider);
        }

        void IService.TryPostInitialize(ServiceProvider provider)
        {
            this.ValidateStatus(ServiceStatus.Initializing);

            this.Status = ServiceStatus.PostInitializing;
            this.PostInitialize(provider);

            this.Status = ServiceStatus.Ready;
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

                this.Status = ServiceStatus.NotReady;

#if DEBUG_VERBOSE
                Console.WriteLine($"Releasing {this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id})...");
                this.OnStatus.Where(kvp => kvp.Value != default).ForEach(kvp => kvp.Value.LogInvocationList($"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id}).{kvp.Key}"));
#endif
            }
        }

        public void TryDispose()
        {
            if (this.Status == ServiceStatus.Ready)
                this.TryRelease();

            if(this.ValidateStatus(status: ServiceStatus.NotReady, required: false))
            {
                this.Status = ServiceStatus.PreDisposing;
                this.PreDispose();

                this.Status = ServiceStatus.Disposing;
                this.Dispose();

                this.Status = ServiceStatus.PostDisposing;
                this.PostDispose();

                this.Status = ServiceStatus.NotCreated;
                this.OnStatus.Clear();
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
            this.Id = Guid.NewGuid();
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
