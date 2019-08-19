using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// An object that can self update using an async thread.
    /// 
    /// This is most often seen within server side scenes
    /// where a single game instance can contain dozens of scenes
    /// running simultaneously. 
    /// </summary>
    public interface IAsyncable : IDriven
    {
        Boolean RunningAsync { get; }

        void TryStartAsync(Int32 delay = 16, Boolean draw = false);

        void TryStopAsync();
    }
}
