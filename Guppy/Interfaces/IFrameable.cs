using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// A frameable object is a guppy child that can be 
    /// updated and drawn every frame.
    /// </summary>
    public interface IFrameable : IInitializable
    {
        #region Public Attributes
        Int32 DrawOrder { get; }
        Int32 UpdateOrder { get; }

        Boolean Visible { get; }
        Boolean Enabled { get; }
        #endregion

        #region Frame Methods
        void TryUpdate(GameTime gameTime);
        void TryDraw(GameTime gameTime);
        #endregion
    }
}
