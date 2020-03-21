using Guppy.Enums;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IService
    {
        #region Attributes
        Guid Id { get; set; }
        ServiceDescriptor Descriptor { get; set; }
        InitializationStatus InitializationStatus { get; }
        #endregion

        #region Events
        event EventHandler OnDisposed;
        #endregion

        #region Methods
        void TryPreInitialize(ServiceProvider provider);
        void TryInitialize(ServiceProvider provider);
        void TryPostInitialize(ServiceProvider provider);

        void TryDispose();
        #endregion
    }
}
