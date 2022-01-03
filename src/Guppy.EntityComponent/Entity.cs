using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent
{
    public class Entity : Service, IEntity
    {
        #region Private Fields
        private ComponentManager _components;
        #endregion

        #region IEntity Implementation
        public ComponentManager Components { get; set; }
        #endregion
    }
}
