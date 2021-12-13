using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Services
{
    public class DataService<TDataConfiguration> : DynamicIdService<TDataConfiguration>
        where TDataConfiguration : DataConfiguration
    {
        private Dictionary<UInt16, TDataConfiguration> _idsConfigurations;
        private Dictionary<Type, TDataConfiguration> _typesConfigurations;

        public readonly TDataConfiguration[] Configurations;

        internal DataService(DynamicIdSize idSize, TDataConfiguration[] configurations) : base(idSize)
        {
            this.Configurations = configurations;

            _idsConfigurations = this.Configurations.ToDictionaryByValue(c => c.Id);
            _typesConfigurations = this.Configurations.ToDictionaryByValue(c => c.DataType);
        }

        public TDataConfiguration ReadConfiguration(NetDataReader im)
        {
            UInt16 id = this.ReadId(im);

            return _idsConfigurations[id];
        }

        public void WriteConfiguration(NetDataWriter om, TDataConfiguration configuration)
        {
            this.WriteId(om, configuration);
        }

        public TDataConfiguration GetConfiguration(UInt16 id)
        {
            return _idsConfigurations[id];
        }

        public TDataConfiguration GetConfiguration(Type dataType)
        {
            return _typesConfigurations[dataType];
        }

        public TDataConfiguration GetConfiguration(IPacket data)
        {
            return _typesConfigurations[data.GetType()];
        }

        public Boolean TryGetConfiguration(UInt16 id, out TDataConfiguration configuration)
        {
            return _idsConfigurations.TryGetValue(id, out configuration);
        }

        public Boolean TryGetConfiguration(Type dataType, out TDataConfiguration configuration)
        {
            return _typesConfigurations.TryGetValue(dataType, out configuration);
        }
    }
}
