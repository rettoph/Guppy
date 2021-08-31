using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Enums;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Components
{
    /// <summary>
    /// Simple helper driver that implement InitializeRemote
    /// and ReleaseRemote methods that will only be invoked 
    /// when the driven item connects to a remote peer
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RemoteHostComponent<TEntity> : Component<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Fields
        private HostType _initialHostType;
        private NetworkAuthorization _initialNetworkAuthorization;
        #endregion

        #region Protected Fields 
        protected HostType initialHostType => _initialHostType;
        protected NetworkAuthorization initialNetworkAuthorization => _initialNetworkAuthorization;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            _initialHostType = provider.Settings.Get<HostType>();
            _initialNetworkAuthorization = provider.Settings.Get<NetworkAuthorization>();
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            if (_initialHostType == HostType.Remote)
                this.InitializeRemote(provider, _initialNetworkAuthorization);
        }

        protected virtual void InitializeRemote(GuppyServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            //
        }

        protected override void Release()
        {
            base.Release();

            if (_initialHostType == HostType.Remote)
                this.ReleaseRemote(_initialNetworkAuthorization);
        }

        protected virtual void ReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            // 
        }
        #endregion
    }
}
