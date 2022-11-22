using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface ISetting
    {
        string Key { get; }
        bool Exportable { get; }
        string[] Tags { get; }
        Type Type { get; }

        void Import(string value);

        string Export();
    }

    public interface ISetting<T> : ISetting
    {
        T Value { get; set; }
        T DefaultValue { get; }
    }
}
