using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IService
    {
        #region Attributes
        ServiceConfiguration ServiceConfiguration { get; set; }
        Guid Id { get; set; }
        #endregion

        #region Events
        event OnEventDelegate<IService> OnReleased;
        #endregion

        #region Methods
        void TryCreate(ServiceProvider provider);
        void TryPreInitialize(ServiceProvider provider);
        void TryInitialize(ServiceProvider provider);
        void TryPostInitialize(ServiceProvider provider);
        void TryRelease();
        void TryDispose();
        #endregion
    }
}
