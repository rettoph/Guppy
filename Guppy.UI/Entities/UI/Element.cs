using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Extensions;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Entities.UI
{
    public abstract class Element : BaseElement
    {
        #region Frame Methods 
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the parent's child hovered value..
            this.Parent.childHovered |= this.Hovered;
        }
        #endregion

        #region Helper Methods
        /// <inheritdoc />
        protected override Boolean GetHovered()
        {
            return this.Parent.Hovered || this.EventType == EventTypes.Normal ? base.GetHovered() : false;
        }
        #endregion
    }
}
