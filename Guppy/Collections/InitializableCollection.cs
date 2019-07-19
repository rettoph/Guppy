using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class InitializableCollection<TInitializable> : UniqueObjectCollection<TInitializable>
        where TInitializable : class, IInitializable
    {
        private Boolean _initializeOnAdd;

        public InitializableCollection(bool disposeOnRemove = true, bool initializeOnAdd = true) : base(disposeOnRemove)
        {
            _initializeOnAdd = initializeOnAdd;
        }

        public override void Add(TInitializable item)
        {
            if (_initializeOnAdd)
            {
                item.TryPreInitialize();
                item.TryInitialize();
                item.TryPostInitialize();
            }

            base.Add(item);
        }

        public override bool Remove(TInitializable item)
        {
            return base.Remove(item);
        }
    }
}
