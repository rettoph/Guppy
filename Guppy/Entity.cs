using Guppy.Configurations;
using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Driven
    {
        #region Public Attributes
        public EntityConfiguration Configuration { get; internal set; }
        #endregion
    }
}
