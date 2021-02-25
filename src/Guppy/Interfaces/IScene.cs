using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IScene : IFrameable
    {
        #region Public Properties
        public LayerList Layers { get; }
        public EntityList Entities { get; }
        #endregion
    }
}
