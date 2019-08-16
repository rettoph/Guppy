using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Reusable objects can be created once then easily
    /// remapped and reconfigured multiple times, reducing the
    /// number of Reflection calls preformed when generating
    /// new services.
    /// </summary>
    public interface IReusable
    {
    }
}
