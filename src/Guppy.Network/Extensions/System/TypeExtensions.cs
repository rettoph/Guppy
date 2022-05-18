using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {
        public static uint xxHash(this Type type)
        {
            return type.AssemblyQualifiedName?.xxHash() ?? throw new Exception();
        }
    }
}
