using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IInitializable : ICreatable
    {
        InitializationStatus Status { get; }

        void TryPreInitialize();
        void TryInitialize();
        void TryPostInitialize();
    }
}
