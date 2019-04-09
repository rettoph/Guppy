using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public class StageContainer : SimpleContainer
    {
        public StageContainer(Stage stage) : base()
        {
            // Update the internal stage
            this.stage = stage;
            // Set the internal bounds
            this.Inner.setParent(this.stage.clientBounds);

            // Ensure the container fills the stage
            this.Inner.X.SetValue(0);
            this.Inner.Y.SetValue(0);
            this.Inner.Width.SetValue(1f);
            this.Inner.Height.SetValue(1f);
        }
    }
}
