using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class StageContent : Container
    {
        public StageContent(UnitRectangle outerBounds, Stage stage, Style style = null) : base(outerBounds, null, stage, style)
        {
            this.setParent(null);
        }
    }
}
