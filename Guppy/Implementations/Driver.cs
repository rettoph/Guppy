using Guppy.Implementations;
using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public abstract class Driver : ZFrameable, IDriver
    {
        public Driver(Driven parent, IServiceProvider provider) : base(provider)
        {
        }
    }
}
