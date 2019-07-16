using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class UniqueObjectCollection<TUniqueObject> : TrackedDisposableCollection<TUniqueObject>
        where TUniqueObject : class, IUniqueObject
    {
        private Dictionary<Guid, TUniqueObject> _table;

        public TUniqueObject this[Guid id]
        {
            get { return this.GetById(id); }
        }

        public UniqueObjectCollection(bool disposeOnRemove = true) : base(disposeOnRemove)
        {
            _table = new Dictionary<Guid, TUniqueObject>();
        }

        public override void Add(TUniqueObject item)
        {
            base.Add(item);

            _table.Add(item.Id, item);
        }

        public override bool Remove(TUniqueObject item)
        {
            if(base.Remove(item))
            {
                _table.Remove(item.Id);

                return true;
            }

            return false;
        }

        public TUniqueObject GetById(Guid id)
        {
            if (_table.ContainsKey(id))
                return _table[id];
            else
                return default(TUniqueObject);
        }

        public TCastedType GetById<TCastedType>(Guid id)
            where TCastedType : class, TUniqueObject
        {
            if (_table.ContainsKey(id))
                return _table[id] as TCastedType;
            else
                return default(TCastedType);
        }
    }
}
