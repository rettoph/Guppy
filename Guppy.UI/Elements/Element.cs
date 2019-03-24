using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public abstract class Element : UnitRectangle
    {
        public Element(Unit x, Unit y, Unit width, Unit height) : base(x, y, width, height)
        {
        }
    }
}
