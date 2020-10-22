using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Events.Delegates;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Service : IService, IDisposable
    {
        #region Private Fields
        private ServiceDescriptor _descriptor;
        private Guid _id;
        #endregion

        #region Public Attributes
        public ServiceDescriptor ServiceDescriptor
        {
            get => _descriptor;
            set
            {
                if (this.InitializationStatus == InitializationStatus.Ready)
                    throw new InvalidOperationException("Unable to update Configuration after initialization.");

                _descriptor = value;
            }
        }
        public virtual Guid Id
        {
            get => _id;
            set
            {
                if (this.InitializationStatus == InitializationStatus.Ready)
                    throw new InvalidOperationException("Unable to update Id after initialization.");

                _id = value;
            }
        }

        public InitializationStatus InitializationStatus { get; private set; }
        #endregion

        #region Events
        public event OnEventDelegate<IService> OnReleased;
        #endregion

        #region Lifecycle Methods
        void IService.TryCreate(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.NotCreated)
                return;

            this.InitializationStatus = InitializationStatus.Creating;
            this.Create(provider);
            this.InitializationStatus = InitializationStatus.NotReady;
        }

        void IService.TryPreInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.NotReady)
                return;

            this.InitializationStatus = InitializationStatus.PreInitializing;
            this.PreInitialize(provider);
        }

        void IService.TryInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.PreInitializing)
                return;

            this.InitializationStatus = InitializationStatus.Initializing;
            this.Initialize(provider);
        }

        void IService.TryPostInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.Initializing)
                return;

            this.InitializationStatus = InitializationStatus.PostInitializing;
            this.PostInitialize(provider);
            this.InitializationStatus = InitializationStatus.Ready;
        }

        public void TryRelease()
        {
            if (this.InitializationStatus != InitializationStatus.Ready && this.InitializationStatus != InitializationStatus.Disposing)
                return;

            this.InitializationStatus = InitializationStatus.Releasing;

            this.Release();
            this.OnReleased?.Invoke(this);

            this.InitializationStatus = InitializationStatus.NotReady;
        }

        public void TryDispose()
        {
            this.TryRelease();

            this.InitializationStatus = InitializationStatus.Disposing;
            this.Dispose();
        }

        protected virtual void Create(ServiceProvider provider)
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

        protected virtual void Release()
        {
            //
        }

        protected virtual void Dispose()
        {
            //
        }
        #endregion

        #region IDisposable Implementation
        void IDisposable.Dispose()
            => this.TryDispose();
        #endregion
    }
}
