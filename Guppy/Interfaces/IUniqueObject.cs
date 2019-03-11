using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IUniqueObject : ITrackedDisposable
    {
        Guid Id { get; }
    }
}
