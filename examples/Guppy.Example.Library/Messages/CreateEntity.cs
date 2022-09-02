using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public readonly struct CreateEntity
    {
        public readonly uint Id;

        public CreateEntity(uint id)
        {
            this.Id = id;
        }
    }
}
