using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Entities.UI
{
    public class Element : BaseElement
    {
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the parent's child hovered value..
            this.Parent.childHovered |= this.Hovered;
        }

        protected override Boolean GetHovered()
        {
            return this.Parent.Hovered || this.EventType == EventTypes.Normal ? base.GetHovered() : false;
        }
    }
}
