using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public interface IPublicContainer<TChildren> : IContainer<TChildren>
        where TChildren : IElement
    {
        void Add(IElement child);
        void Remove(IElement child);
    }
}
