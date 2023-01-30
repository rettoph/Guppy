using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        void Serialize<T>(Stream utf8Json, T obj);

        T? Deserialize<T>(string json);
        T? Deserialize<T>(Stream utf8Json);
    }
}
