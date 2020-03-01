using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IOrderable : IDriven
    {
        Int32 DrawOrder { get; set; }
        Int32 UpdateOrder { get; set; }

        event EventHandler<Int32> OnDrawOrderChanged;
        event EventHandler<Int32> OnUpdateOrderChanged;
    }
}
