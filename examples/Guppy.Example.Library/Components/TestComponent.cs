using Guppy.EntityComponent;
using Guppy.Gaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components
{
    public class TestComponent : Component<Scene>
    {
        public TestComponent(Scene entity) : base(entity)
        {
        }
    }
}
