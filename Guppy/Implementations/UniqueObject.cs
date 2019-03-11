using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public abstract class UniqueObject : TrackedDisposable, IUniqueObject
    {
        public Guid Id { get; private set; }

        public UniqueObject(Guid id)
        {
            this.Id = id;
        }
        public UniqueObject()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
