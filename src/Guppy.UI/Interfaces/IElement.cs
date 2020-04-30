using Guppy.Interfaces;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IElement : IService, IFrameable
    {
        #region Attributes
        SpriteBatch SpriteBatch { get; }
        UnitRectangle Bounds { get; }
        Boolean Hovered { get; }
        Boolean Active { get; }
        #endregion
    }
}
