using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class EntityCollection : FrameableCollection<Entity>
    {
        public EntityCollection(IServiceProvider provider) : base(provider)
        {
        }
    }
}
