using Guppy.DependencyInjection;
using Guppy.Utilities;
using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Core
{
    public static class Constants
    {
        public static class ServiceConfigurationKeys
        {
            public static readonly ServiceConfigurationKey ILog = ServiceConfigurationKey.From<ILog>();
        }

        public static class Priorities
        {
            public const Int32 PreCreate = -10;
            public const Int32 Create = 10;
            public const Int32 PostCreate = 20;

            public const Int32 PreInitialize = -10;
            public const Int32 Initialize = 10;
            public const Int32 PostInitialize = 20;
        }

        public static class Orders
        {
            public const Int32 ComponentOrder = -10;
        }
    }
}
