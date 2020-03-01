using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IAsyncable : IDriven
    {
        Boolean RunningAsync { get; }

        void TryStartAsync(Int32 delay = 16, Boolean draw = false);

        void TryStopAsync();
    }
}
