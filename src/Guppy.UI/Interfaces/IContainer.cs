using Guppy.Lists;
using Guppy.UI.Enums;
using Guppy.UI.Lists;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer
    {
        #region Methods
        /// <summary>
        /// Retrieve the bounds to pass into the current containers
        /// children when recalculating child bounds.
        /// </summary>
        /// <returns></returns>
        Rectangle GetInnerBoundsForChildren();
        #endregion
    }

    public interface IContainer<TChildren> : IContainer
        where TChildren : class, IElement
    {
        #region Properties
        InlineType Inline { get; set; }
        ElementList<TChildren> Children { get; }
        #endregion
    }
}
