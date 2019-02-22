using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILivingObject : IDrawable, IUpdateable, IInitializable
    {
        void SetEnabled(Boolean enabled);
        void SetVisible(Boolean visible);

        void SetUpdateOrder(Int32 updateOrder);
        void SetDrawOrder(Int32 drawOrder);
    }
}
