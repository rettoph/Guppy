using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal interface ISettingValue
    {
        Setting Setting { get; }
        object Value { get; set; }
    }
}
