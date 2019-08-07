using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Defines asyncable objects that can (but dont have to be)
    /// looped through in an async manner.
    /// </summary>
    public interface IAsyncable : IDriven
    {
        Boolean RunningAsync { get; }

        void TryStartAsync(Int32 delay = 16, Boolean draw = false);

        void TryStopAsync();
    }
}
