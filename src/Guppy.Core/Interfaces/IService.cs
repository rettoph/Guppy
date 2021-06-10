using Guppy.DependencyInjection;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Enums;
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
        ServiceStatus Status { get; }
        IServiceConfiguration ServiceConfiguration { get; set; }
        Guid Id { get; set; }
        #endregion

        #region Events
        Dictionary<ServiceStatus, OnChangedEventDelegate<IService, ServiceStatus>> OnStatus { get; }
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
