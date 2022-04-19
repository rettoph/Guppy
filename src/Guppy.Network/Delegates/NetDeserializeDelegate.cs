﻿using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Delegates
{
    public delegate void NetDeserializeDelegate<T>(NetDataReader reader, out T instance);
}