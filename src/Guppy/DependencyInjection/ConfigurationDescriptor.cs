﻿using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public sealed class ConfigurationDescriptor
    {
        #region Public Attributes
        public String Name { get; set; }
        public Type ServiceType { get; set; }
        public Func<Object, ServiceProvider, ServiceFactory, Object> Configure { get; set; }
        public Int32 Priority { get; set; }
        #endregion
    }
}
