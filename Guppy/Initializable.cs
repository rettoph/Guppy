using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    #region InitiailzationStatus Enum
    public enum InitializationStatus
    {
        NotInitialized,
        Initializing,
        Ready,
        Disposing
    }
    #endregion

    public class Initializable : Creatable, IInitializable
    {
        #region Private Fields
        private InitializationStatus _initializationStatus;
        #endregion

        #region Public Attributes
        public InitializationStatus Status { get => _initializationStatus; }
        #endregion

        #region Constructor
        public Initializable()
        {
            _initializationStatus = InitializationStatus.NotInitialized;
        }
        #endregion

        #region Initialization Methods
        public void TryPreInitialize()
        {
            if (_initializationStatus == InitializationStatus.NotInitialized)
            {
                _initializationStatus = InitializationStatus.Initializing;
                this.PreInitialize();
            }
            else
                throw new Exception($"Unable to pre initialize, InitializationStatus is {_initializationStatus} but {InitializationStatus.NotInitialized} is required.");
        }

        public void TryInitialize()
        {
            if (_initializationStatus == InitializationStatus.Initializing)
            {
                this.Initialize();
            }
            else
                throw new Exception($"Unable to initialize, InitializationStatus is {_initializationStatus} but {InitializationStatus.Initializing} is required.");
        }

        public void TryPostInitialize()
        {
            if (_initializationStatus == InitializationStatus.Initializing)
            {
                this.PostInitialize();

                _initializationStatus = InitializationStatus.Ready;
            }
            else
                throw new Exception($"Unable to post initialize, InitializationStatus is {_initializationStatus} but {InitializationStatus.Initializing} is required.");
        }

        public override void Dispose()
        {
            _initializationStatus = InitializationStatus.Disposing;

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
            if (_initializationStatus > InitializationStatus.Initializing)
                throw new Exception("Unable to update id after initialization!");

            this.Id = id;
        }
        #endregion
    }
}
