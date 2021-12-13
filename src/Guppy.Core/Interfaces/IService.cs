using DotNetUtils.DependencyInjection;
using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IService
    {
        #region Properties
        ServiceStatus Status { get; }
        ServiceConfiguration<GuppyServiceProvider> ServiceConfiguration { get; set; }
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
