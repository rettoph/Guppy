using Guppy.Contexts;
using Guppy.LayerGroups;
using Guppy.EntityComponent.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILayer : IOrderable
    {
        #region Properties
        OrderableList<ILayerable> Items { get; }
        LayerGroup Group { get; set; }
        #endregion

        #region Methods
        void SetContext(LayerContext context);
        #endregion
    }
}
