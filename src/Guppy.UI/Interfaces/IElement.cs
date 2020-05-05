using Guppy.Interfaces;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Backgrounds;
using Guppy.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IElement : IService, IFrameable
    {
        #region Attributes
        Background Background { get; set; }
        SpriteBatch SpriteBatch { get; }
        UnitRectangle Bounds { get; }
        Boolean Hovered { get; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnHoveredChanged;
        event EventHandler<Background> OnBackgroundChanged;
        #endregion
    }
}
