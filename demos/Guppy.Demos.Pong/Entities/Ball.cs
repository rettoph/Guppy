using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Enums;

namespace Guppy.Demos.Pong.Entities
{
    [Service(Lifetime.Transient)]
    public class Ball : Entity
    {
    }
}
