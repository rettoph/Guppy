using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Creatable objects can be created once then easily
    /// remapped and reconfigured multiple times, reducing the
    /// number of Reflection calls preformed when generating
    /// new instances.
    /// 
    /// The CreatablePool object can be used to easily pool
    /// instances of this type together for reuse. That being
    /// said, it is not required that this type be reused as
    /// seen in Scene and Game instances.
    /// </summary>
    public interface ICreateable : IUnique
    {
        void TryPreCreate(IServiceProvider provider);
        void TryCreate(IServiceProvider provider);
        void TryPostCreate(IServiceProvider provider);
    }
}
