using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    #region InitiailzationStatus Enum
    internal enum InitializationStatus
    {
        NotInitialized,
        Initializing,
        Ready
    }
    #endregion

    public class Initializable : Creatable
    {
        #region Private Fields
        private InitializationStatus _initializationStatus;
        #endregion

        #region Constructor
        public Initializable()
        {
            _initializationStatus = InitializationStatus.NotInitialized;
        }
        #endregion

        #region Initialization Methods
        internal void TryInitialize()
        {
            if (_initializationStatus == InitializationStatus.NotInitialized)
            {
                _initializationStatus = InitializationStatus.Initializing;
                this.PreInitialize();
                this.Initialize();
                this.PostInitialize();

                _initializationStatus = InitializationStatus.Ready;
            }
            else
                throw new Exception($"Unable to initialize, InitializationStatus is {_initializationStatus} but {InitializationStatus.NotInitialized} is required.");
        }

        public override void Dispose()
        {
            base.Dispose();

            _initializationStatus = InitializationStatus.NotInitialized;
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
        #endregion

        #region Helper Methods
        public void SetId(Guid id)
        {
            if (_initializationStatus != InitializationStatus.NotInitialized)
                throw new Exception("Unable to update id after initialization!");

            this.Id = id;
        }
        #endregion
    }
}
