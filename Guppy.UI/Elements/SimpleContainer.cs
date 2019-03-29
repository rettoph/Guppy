using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Entities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Containers are specific elements that can contain
    /// other elements.
    /// </summary>
    public class SimpleContainer : Container
    {
        public override Stage Stage { get { return this.Parent.Stage; } }

        public SimpleContainer(Unit x, Unit y, Unit width, Unit height) : base(x, y, width, height)
        {
            this.children = new List<Element>();
        }
    }
}
