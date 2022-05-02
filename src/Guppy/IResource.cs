using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public interface IResource
    {
        string Key { get; }
        Type ValueType { get; }

        object? GetDefaultValue();
        object? GetValue();
        bool TrySetValue(object value);
        void Reset();
        string? Export();
        void Import(string? value);
    }
}
