using Guppy.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Constants
{
    public static class EntityConstants
    {
        public static readonly EntityKey Ship = new EntityKey("paddle");

        public static class Tags
        {
            public static readonly EntityTag Ship = new EntityTag("paddle");
            public static readonly EntityTag Movable = new EntityTag("movable");
            public static readonly EntityTag Network = new EntityTag("network");
        }
    }
}
