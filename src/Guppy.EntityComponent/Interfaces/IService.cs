using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.Interfaces
{
    public interface IService
    {
        #region Properties
        ServiceStatus Status { get; }
        ServiceConfiguration ServiceConfiguration { get; set; }
        Guid Id { get; set; }
        #endregion

        #region Events
        event OnChangedEventDelegate<IService, ServiceStatus> OnStatusChanged;
        #endregion

        #region Lifecycle Methods
        void TryPreCreate(ServiceProvider provider);
        void TryCreate(ServiceProvider provider);
        void TryPostCreate(ServiceProvider provider);
        void TryPreInitialize(ServiceProvider provider);
        void TryInitialize(ServiceProvider provider);
        void TryPostInitialize(ServiceProvider provider);
        void TryRelease();
        void TryDispose();
        #endregion
    }
}
