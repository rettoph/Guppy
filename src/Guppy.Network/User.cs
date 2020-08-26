using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Network.Collections;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network
{
    public class User : Service, INetworkService
    {
        #region Public Attributes
        public Dictionary<String, String> Claims;
        public UserGroupCollection Groups { get; private set; }
        public String Name
        {
            get => this.Claims["name"];
            set => this.Claims["name"] = value;
        }
        #endregion

        #region Constructor
        internal User()
        {

        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Groups = provider.GetService<UserGroupCollection>();

            this.Claims = new Dictionary<String, String>();
            this.Claims["name"] = String.Empty;
        }
        #endregion

        #region INetworkService Implementation
        public void TryRead(NetIncomingMessage im)
        {
            var claims = im.ReadInt32();
            this.Claims = new Dictionary<String, String>(claims);
            for (Int32 i = 0; i < claims; i++)
                this.Claims.Add(im.ReadString(), im.ReadString());
        }

        public void TryWrite(NetOutgoingMessage om)
        {
            om.Write(this.Claims.Count);
            this.Claims.ForEach(kvp =>
            {
                om.Write(kvp.Key);
                om.Write(kvp.Value);
            });
        }
        #endregion
    }
}
