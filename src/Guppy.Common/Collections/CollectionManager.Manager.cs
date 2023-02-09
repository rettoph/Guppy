using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public partial class CollectionManager<T>
    {
        private abstract class Manager
        {
            public readonly Type Type;
            public readonly IEnumerable Collection;

            protected Manager(Type type, IEnumerable collection)
            {
                this.Type = type;
                this.Collection = collection;
            }

            public abstract bool TryAdd(T instance);
            public abstract bool TryRemove(T instance);
            public abstract void Clear();
        }

        private class Manager<TCollection> : Manager
        {
            public new readonly ICollection<TCollection> Collection;

            public Manager(ICollection<TCollection> collection) : base(typeof(TCollection), collection)
            {
                this.Collection = collection;
            }

            public override void Clear()
            {
                this.Collection.Clear();
            }

            public override bool TryAdd(T instance)
            {
                if(instance is TCollection casted)
                {
                    this.Collection.Add(casted);
                    return true;
                }

                return false;
            }

            public override bool TryRemove(T instance)
            {
                if (instance is TCollection casted)
                {
                    return this.Collection.Remove(casted);
                }

                return false;
            }
        }
    }
}
