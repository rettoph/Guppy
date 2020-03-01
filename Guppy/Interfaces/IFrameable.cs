using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IFrameable : IInitializable
    {
        event EventHandler<Boolean> OnVisibleChanged;
        event EventHandler<Boolean> OnEnabledChanged;

        Boolean Enabled { get; set; }
        Boolean Visible { get; set; }

        void TryDraw(GameTime gameTime);
        void TryUpdate(GameTime gameTime);
    }
}
