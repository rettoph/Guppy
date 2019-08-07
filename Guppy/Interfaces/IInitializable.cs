using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IInitializable : IDisposable
    {
        #region Public Attributes
        InitializationStatus InitializationStatus { get; }
        #endregion

        #region Lifecycle Methods
        void TryPreInitialize();
        void TryInitialize();
        void TryPostInitialize();
        #endregion
    }
}
