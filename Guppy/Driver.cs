using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy
{
    public abstract class Driver : Frameable, IDriver
    {
        #region Public Attributes
        public IDriven Driven { get; private set; }
        #endregion

        #region Constructor
        public Driver(IDriven driven)
        {
            this.Driven = driven;
        }
        #endregion
    }

}