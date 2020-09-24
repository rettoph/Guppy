using Guppy.UI.Lists;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IBaseContainer : IElement
    {
        /// <summary>
        /// Return the location of the container that all 
        /// children should utilize for positioning.
        /// </summary>
        /// <returns></returns>
        Point GetContainerLocation();
        /// <summary>
        /// Return the size of the container that all 
        /// children should utilize for sizing.
        /// </summary>
        /// <returns></returns>
        Point GetContainerSize();
    }
}
