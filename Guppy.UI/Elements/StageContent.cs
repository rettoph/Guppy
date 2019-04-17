using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class StageContent : Container
    {
        public override Stage Stage { get; }

        public StageContent(Stage stage, Unit x, Unit y, Unit width, Unit height, Style style = null) : base(x, y, width, height, style)
        {
            this.Stage = stage;
            this.setParent(null);
        }
    }
}
