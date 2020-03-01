using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IConfigurable : IOrderable
    {
        String Handle { get; set; }
        String Name { get; set; }
        String Description { get; set; }
    }
}
