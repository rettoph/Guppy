﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Configurations
{
    public class EntityConfiguration
    {
        public String Handle { get; internal set; }
        public String Name { get; internal set; }
        public String Description { get; internal set; }
        public Object CustomData { get; internal set; }
        public Type Type { get; internal set; }
    }
}