﻿using Guppy.Network.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetSerializers
{
    internal sealed class RuntimeNetSerializerDefinition<T> : NetSerializerDefinition
    {
        public override Type Type => typeof(T);

        private NetSerializeDelegate<T> _serialize;
        private NetDeserializeDelegate<T> _deserialize;

        public RuntimeNetSerializerDefinition(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public override NetSerializer Build(INetId id)
        {
            return new NetSerializer<T>(id, _serialize, _deserialize);
        }
    }
}
