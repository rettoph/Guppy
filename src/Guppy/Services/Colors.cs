using Guppy.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Services
{
    [AutoLoad]
    public class Colors : LoaderService<String, Color>
    {
    }
}
