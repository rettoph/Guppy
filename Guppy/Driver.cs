using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Driver : LivingObject
    {
        public Driver(Driven entity, ILogger logger) : base(logger)
        {
        }
    }
}
