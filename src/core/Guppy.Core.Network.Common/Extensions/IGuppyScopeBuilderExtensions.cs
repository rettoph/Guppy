using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterPeerTypeFilter(this IGuppyScopeBuilder builder, PeerTypeEnum peerType, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequirePeerType(peerType), build);
        }

        /// <summary>
        /// Configure scoped <see cref="Guppy.Core.Network.Common.INetScope{T}"/> instance.
        /// Traditionally this is done on scope creation with a custom builder action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="peerType"></param>
        /// <param name="groupId"></param>
        public static IGuppyScopeBuilder RegisterNetScope<T>(this IGuppyScopeBuilder builder, PeerTypeEnum peerType, byte groupId)
        {
            builder.Variables.Add(GuppyNetworkVariables.Scope.PeerType.Create(peerType));
            builder.RegisterInstance<NetScopeContext<T>>(new NetScopeContext<T>(peerType, groupId)).SingleInstance();
            builder.Register<INetScope>(ctc => ctc.Resolve<INetScope<T>>()).InstancePerLifetimeScope();

            return builder;
        }
    }
}