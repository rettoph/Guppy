using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Service : IService
    {
        #region Private Fields
        private ServiceConfiguration _configuration;
        private Guid _id;
        private ServiceDescriptor _descriptor;
        #endregion

        #region Public Attributes
        public ServiceConfiguration ServiceConfiguration
        {
            get => _configuration;
            set
            {
                if (this.InitializationStatus == InitializationStatus.Ready)
                    throw new InvalidOperationException("Unable to update Configuration after initialization.");

                _configuration = value;
            }
        }
        public Guid Id
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
        public event GuppyEventHandler<IService> OnDisposed;
        #endregion

        #region Lifecycle Methods
        public void TryPreInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.NotReady)
                throw new InvalidOperationException("Unable to PreInitialize service.");

            this.InitializationStatus = InitializationStatus.PreInitializing;
            this.PreInitialize(provider);
        }

        public void TryInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.PreInitializing)
                throw new InvalidOperationException("Unable to Initialize service.");

            this.InitializationStatus = InitializationStatus.Initializing;
            this.Initialize(provider);
        }

        public void TryPostInitialize(ServiceProvider provider)
        {
            if (this.InitializationStatus != InitializationStatus.Initializing)
                throw new InvalidOperationException("Unable to PostInitialize service.");

            this.InitializationStatus = InitializationStatus.PostInitializing;
            this.PostInitialize(provider);
            this.InitializationStatus = InitializationStatus.Ready;
        }

        public void TryDispose()
        {
            this.InitializationStatus = InitializationStatus.Disposing;

            this.Dispose();
            this.OnDisposed?.Invoke(this);

            this.InitializationStatus = InitializationStatus.NotReady;
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

        protected virtual void Dispose()
        {
            //
        }
        #endregion
    }
}
