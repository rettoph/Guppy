using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer<TChildren> : IElement
        where TChildren : class, IElement
    {
        #region Properties
        ServiceList<TChildren> Children { get; }
        #endregion
    }
}
