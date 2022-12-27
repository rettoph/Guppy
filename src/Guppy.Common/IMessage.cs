using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IMessage
    {
        Type Type { get; }
    }
}
