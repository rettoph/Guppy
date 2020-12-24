using Guppy.Lists;
using Guppy.UI.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer : IElement
    {

    }

    public interface IContainer<TChildren> : IContainer
        where TChildren : class, IElement
    {
        #region Properties
        ElementList<TChildren> Children { get; }
        #endregion
    }
}
