using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    /// <summary>
    /// Simple helper driver that implement InitializeRemote
    /// and ReleaseRemote methods that will only be invoked 
    /// when the driven item connects to a remote peer
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class NetworkComponent<TEntity> : Component<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Fields
        private HostType _initialHostType;
        private NetworkAuthorization _initialNetworkAuthorization;
        #endregion

        #region Protected Fields 
        protected HostType hostType => _initialHostType;
        protected NetworkAuthorization networkAuthorization => _initialNetworkAuthorization;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _initialHostType = provider.Settings.Get<HostType>();
            _initialNetworkAuthorization = provider.Settings.Get<NetworkAuthorization>();

            if (_initialHostType == HostType.Remote)
                this.PreInitializeRemote(provider, _initialNetworkAuthorization);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            if (_initialHostType == HostType.Remote)
                this.InitializeRemote(provider, _initialNetworkAuthorization);
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            if (_initialHostType == HostType.Remote)
                this.PostInitializeRemote(provider, _initialNetworkAuthorization);
        }


        protected override void PreRelease()
        {
            base.PreRelease();

            if (_initialHostType == HostType.Remote)
                this.PreReleaseRemote(_initialNetworkAuthorization);
        }

        protected override void Release()
        {
            base.Release();

            if (_initialHostType == HostType.Remote)
                this.ReleaseRemote(_initialNetworkAuthorization);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            if (_initialHostType == HostType.Remote)
                this.PostReleaseRemote(_initialNetworkAuthorization);
        }

        protected virtual void PreInitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            //
        }

        protected virtual void InitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            //
        }

        protected virtual void PostInitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            //
        }

        protected virtual void PreReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            // 
        }

        protected virtual void ReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            // 
        }

        protected virtual void PostReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            // 
        }
        #endregion
    }
}
