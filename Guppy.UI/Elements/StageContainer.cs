using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class StageContainer : Container
    {
        public override Stage Stage { get; }

        public StageContainer(Stage stage, Unit x, Unit y, Unit width, Unit height) : base(x, y, width, height)
        {
            this.Stage = stage;
        }
    }
}
