using Guppy.Enums;
using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public abstract class Initializable : UniqueObject, IInitializable
    {
        #region Proteced Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public InitializationStatus InitializationStatus { get; private set; }
        #endregion

        #region Constructors
        public Initializable(ILogger logger)
        {
            this.InitializationStatus = InitializationStatus.NotReady;

            this.logger = logger;
        }
        #endregion

        #region Internal Initialization Methods
        protected virtual void Boot()
        {
            this.InitializationStatus = InitializationStatus.Booting;
        }

        protected virtual void PreInitialize()
        {
            this.InitializationStatus = InitializationStatus.PreInitializing;
        }

        protected virtual void Initialize()
        {
            this.InitializationStatus = InitializationStatus.Initializing;
        }

        protected virtual void PostInitialize()
        {
            this.InitializationStatus = InitializationStatus.PostInitializing;
        }
        #endregion

        #region Initialization Methods
        public void TryBoot()
        {
            if(this.InitializationStatus != InitializationStatus.NotReady)
            { // Invalid Initialization Status... Log this error.
                this.logger.LogError($"Initialization Error! Unable to boot, current InitializationStatus is {this.InitializationStatus}. InitializationStatus.NotReady is expected.");
            }
            else
            { // Valid Initialization Status. Call Boot.
                this.Boot();
            }
        }

        public void TryPreInitialize()
        {
            if (this.InitializationStatus != InitializationStatus.Booting)
            { // Invalid Initialization Status... Log this error.
                this.logger.LogError($"Initialization Error! Unable to pre-initialize, current InitializationStatus is {this.InitializationStatus}. InitializationStatus.Booting is expected.");
            }
            else
            { // Valid Initialization Status. Call PreInitialize.
                this.PreInitialize();
            }
        }

        public void TryInitialize()
        {
            if (this.InitializationStatus != InitializationStatus.PreInitializing)
            { // Invalid Initialization Status... Log this error.
                this.logger.LogError($"Initialization Error! Unable to initialize, current InitializationStatus is {this.InitializationStatus}. InitializationStatus.PreInitializing is expected.");
            }
            else
            { // Valid Initialization Status. Call Initialize.
                this.Initialize();
            }
        }

        public void TryPostInitialize()
        {
            if (this.InitializationStatus != InitializationStatus.Initializing)
            { // Invalid Initialization Status... Log this error.
                this.logger.LogError($"Initialization Error! Unable to post-initialize, current InitializationStatus is {this.InitializationStatus}. InitializationStatus.Initializing is expected.");
            }
            else
            { // Valid Initialization Status. Call PostInitialize.
                this.PostInitialize();

                // after the system is done post initializing, we can mark the current object as ready
                this.InitializationStatus = InitializationStatus.Ready;
            }
        }
        #endregion
    }
}
