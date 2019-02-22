using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IInitializable : ITrackedDisposable
    {
        InitializationStatus InitializationStatus { get; }

        void TryBoot();
        void TryPreInitialize();
        void TryInitialize();
        void TryPostInitialize();
    }
}
