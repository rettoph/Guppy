using Guppy.Lists;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Lists
{
    public class ElementList<TElement> : ServiceList<TElement>
        where TElement : class, IElement
    {
    }
}
