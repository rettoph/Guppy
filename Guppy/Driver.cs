using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy
{
    public abstract class Driver : Frameable
    {
        #region Constructor
        public Driver(Driven driven)
        {
        }
        #endregion
    }
}
