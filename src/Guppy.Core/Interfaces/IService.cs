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
        event OnChangedEventDelegate<IService, ServiceStatus> OnStatusChanged;
        #endregion

        #region Lifecycle Methods
        void TryPreCreate(GuppyServiceProvider provider);
        void TryCreate(GuppyServiceProvider provider);
        void TryPostCreate(GuppyServiceProvider provider);
        void TryPreInitialize(GuppyServiceProvider provider);
        void TryInitialize(GuppyServiceProvider provider);
        void TryPostInitialize(GuppyServiceProvider provider);
        void TryRelease();
        void TryDispose();
        #endregion
    }
}
