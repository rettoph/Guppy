﻿using Guppy.Attributes;
using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Definitions.SettingSerializers
{
    [AutoLoad]
    internal sealed class FloatSettingSerializer : SettingSerializerDefinition<float>
    {
        public override float Deserialize(string serialized)
        {
            return float.Parse(serialized);
        }

        public override string Serialize(float deserialized)
        {
            return deserialized.ToString();
        }
    }
}