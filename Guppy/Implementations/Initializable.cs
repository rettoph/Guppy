using Guppy.Enums;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class Initializable : IInitializable, IDisposable
    {
        #region Public Attributes
        public InitializationStatus InitializationStatus { get; private set; }
        #endregion

        #region Initialization Methods
        public void TryPreInitialize()
        {
            if (this.InitializationStatus == InitializationStatus.NotInitialized)
            {
                this.InitializationStatus = InitializationStatus.PreInitializing;
                this.PreInitialize();
            }
            else
                throw new Exception($"Unable to pre initialize, InitializationStatus is {this.InitializationStatus} but {InitializationStatus.NotInitialized} is required.");
        }

        public void TryInitialize()
        {
            if (this.InitializationStatus == InitializationStatus.PreInitializing)
            {
                this.InitializationStatus = InitializationStatus.Initializing;
                this.Initialize();
            }
            else
                throw new Exception($"Unable to initialize, InitializationStatus is {this.InitializationStatus} but {InitializationStatus.PreInitializing} is required.");
        }

        public void TryPostInitialize()
        {
            if (this.InitializationStatus == InitializationStatus.Initializing)
            {
                this.InitializationStatus = InitializationStatus.PostInitializing;
                this.PostInitialize();
                this.InitializationStatus = InitializationStatus.Ready;
            }
            else
                throw new Exception($"Unable to pre initialize, InitializationStatus is {this.InitializationStatus} but {InitializationStatus.Initializing} is required.");
        }

        protected virtual void PreInitialize()
        {
            // 
        }

        protected virtual void Initialize()
        {
            // 
        }

        protected virtual void PostInitialize()
        {
            // 
        }

        public virtual void Dispose()
        {
            this.InitializationStatus = InitializationStatus.NotInitialized;
        }
        #endregion
    }
}
