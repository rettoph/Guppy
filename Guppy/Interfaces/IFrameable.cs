﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// A frameable object is a guppy child that can be 
    /// updated and drawn every frame.
    /// </summary>
    public interface IFrameable : ICreateable
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

        #region Helper Methods
        void SetUpdateOrder(Int32 value);
        void SetDrawOrder(Int32 value);
        void SetVisible(Boolean value);
        void SetEnabled(Boolean value);
        #endregion
    }
}
