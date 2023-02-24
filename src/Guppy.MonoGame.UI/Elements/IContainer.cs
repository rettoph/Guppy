using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public interface IContainer : IElement
    {
        IEnumerable<IElement> Children { get; }
    }

    public interface IContainer<TChildren> : IContainer
        where TChildren : IElement
    {
        new ReadOnlyCollection<TChildren> Children { get; }
    }
}
