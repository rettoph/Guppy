using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IDriver : IFrameable
    {
        IDriven Driven { get; set; }
    }
}
