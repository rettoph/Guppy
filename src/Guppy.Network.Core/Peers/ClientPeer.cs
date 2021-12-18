using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Dtos;
using Guppy.Network.Enums;
using Guppy.Network.Security.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class ClientPeer : Peer
    {
        protected override void PreCreate(ServiceProvider provider)
        {
            base.PreCreate(provider);

            provider.Settings.Set(NetworkAuthorization.Slave);
        }

        #region Connect Methods
        public void Connect(String target, Int32 port, params Claim[] claims)
        {
            this.UsingDataWriter(writer =>
            {
                ConnectionRequestDto connectionRequest = new ConnectionRequestDto()
                {
                    NetworkProvider = this.network.ToDto(),
                    Claims = claims
                };

                this.network.GetDataTypeConfiguration<ConnectionRequestDto>().Writer(writer, connectionRequest);

                this.manager.Connect(target, port, writer);
            });
        }
        #endregion
    }
}
