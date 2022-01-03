using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.Interfaces
{
    public interface IService : IDisposable
    {
        #region Properties
        ServiceStatus Status { get; }
        ServiceConfiguration ServiceConfiguration { get; internal set; }
        Guid Id { get; }
        #endregion

        #region Events
        event OnChangedEventDelegate<IService, ServiceStatus> OnStatusChanged;
        #endregion

        #region Lifecycle Methods
        internal void TryPreInitialize(ServiceProvider provider);
        internal void TryInitialize(ServiceProvider provider);
        internal void TryPostInitialize(ServiceProvider provider);
        #endregion
    }
}
