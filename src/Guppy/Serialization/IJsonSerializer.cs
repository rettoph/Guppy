using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Serialization
{
    public interface IJsonSerializer
    {
        T? Deserialize<T>(string json);
        string Serialize<T>(T value);
    }
}
