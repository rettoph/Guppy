using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IInitializable : ICreateable
    {
        void TryPreInitialize();
        void TryInitialize();
        void TryPostInitialize();
    }
}
