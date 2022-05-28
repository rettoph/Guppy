using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Definitions.SettingSerializers;
using Guppy.Network.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class SettingsServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSettingSerializer<HostTypeSettingSerializer>();
            services.AddSettingSerializer<NetworkAuthorizationSettingSerializer>();

            services.AddSetting(HostType.Local            , false, SettingConstants.NetworkTag);
            services.AddSetting(NetworkAuthorization.Slave, false, SettingConstants.NetworkTag);

            services.AddSetting(SettingConstants.MaxNetOutgoingMessageRecipients, 256, false, SettingConstants.NetworkTag);
            services.AddSetting(SettingConstants.MaxRoomUsers, 256, false, SettingConstants.NetworkTag);
        }
    }
}
