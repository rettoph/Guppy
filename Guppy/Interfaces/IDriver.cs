using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Drivers are objects that reside within an 
    /// </summary>
    public interface IDriver : IFrameable
    {
        void SetParent(IDriven driven);
    }
}
