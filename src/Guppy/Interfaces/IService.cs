using Guppy.DependencyInjection;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    #region Delegates
    public delegate void GuppyEventHandler<TSender>(TSender sender);
    public delegate void GuppyEventHandler<TSender, TArg>(TSender sender, TArg arg);
    public delegate void GuppyDeltaEventHandler<TSender, TArg>(TSender sender, TArg old, TArg value);
    #endregion

    public interface IService
    {
        #region Attributes
        ServiceDescriptor ServiceDescriptor { get; set; }
        Guid Id { get; set; }
        #endregion

        #region Events
        event GuppyEventHandler<IService> OnDisposed;
        #endregion

        #region Methods
        void TryPreInitialize(ServiceProvider provider);
        void TryInitialize(ServiceProvider provider);
        void TryPostInitialize(ServiceProvider provider);

        void TryDispose();
        #endregion
    }
}
