using Guppy.Contexts;
using Guppy.LayerGroups;
using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILayer : IOrderable
    {
        #region Properties
        OrderableList<IEntity> Entities { get; }
        LayerGroup Group { get; set; }
        #endregion

        #region Methods
        void SetContext(LayerContext context);
        #endregion
    }
}
