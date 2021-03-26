using Guppy.Extensions.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class GuppyNetworkConstants
    {
        public static class Messages
        {
            public static class NetworkScene
            {
                public static UInt32 CreateEntity = "scene:entity:create".xxHash();
                public static UInt32 UpdateEntity = "scene:entity:create".xxHash();
                public static UInt32 RemoveEntity = "scene:entity:remove".xxHash();
            }

            public static class NetworEntity
            {
                public static UInt32 Create = "entity:create".xxHash();
                public static UInt32 Update = "entity:create".xxHash();
                public static UInt32 Remove = "entity:remove".xxHash();
            }
        }
    }
}
