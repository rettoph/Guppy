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
        ServiceContext ServiceContext { get; set; }
        Guid Id { get; set; }
        #endregion

        #region Events
        event OnEventDelegate<IService> OnReleased;
        #endregion

        #region Methods
        void TryCreate(GuppyServiceProvider provider);
        void TryPreInitialize(GuppyServiceProvider provider);
        void TryInitialize(GuppyServiceProvider provider);
        void TryPostInitialize(GuppyServiceProvider provider);
        void TryRelease();
        void TryDispose();
        #endregion
    }
}
