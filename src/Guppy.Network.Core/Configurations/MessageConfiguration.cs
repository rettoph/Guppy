using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class MessageConfiguration
    {
        public readonly DynamicId Id;

        public readonly String Name;

        public readonly Type DataType;

        public readonly Action<NetDataWriter, IData> DataWriter;

        public readonly Func<NetDataReader, IData> DataReader;

        internal MessageConfiguration(
            DynamicId id,
            String name,
            Type dataType,
            Action<NetDataWriter, IData> dataWriter,
            Func<NetDataReader, IData> dataReader)
        {
            this.Id = id;
            this.Name = name;
            this.DataType = dataType;
            this.DataWriter = dataWriter;
            this.DataReader = dataReader;
        }
    }
}
