using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public interface ICollectionManager<T> : ICollection<T>
    {
        public IEnumerable<IEnumerable> Collections { get; }

        public ICollection<TItem> Collection<TItem>();

        ICollectionManager<T> Attach<TItem>(ICollection<TItem> collection);

        ICollectionManager<T> Detach<TItem>(ICollection<TItem> collection);

        ICollectionManager<T> AddRange(IEnumerable<T> items);
    }
}
