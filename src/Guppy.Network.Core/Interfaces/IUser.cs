using Guppy.Interfaces;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface IUser : IService
    {
        /// <summary>
        /// <para>The Users direct connection, if known.</para>
        /// 
        /// <para>On the server this is always defined.</para>
        /// 
        /// <para>On the client only the local user and server user connection values are defined.</para>
        /// </summary>
        NetConnection Connection { get; internal set; }

        String this[String claim] { get; }

        IReadOnlyDictionary<String, Claim> Claims { get; }

        void AddClaim(Claim claim);
        void SetClaim(Claim claim);
    }
}
