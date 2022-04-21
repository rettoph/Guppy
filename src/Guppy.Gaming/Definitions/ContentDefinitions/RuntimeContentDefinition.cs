using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions.ContentDefinitions
{
    internal sealed class RuntimeContentDefinition<T> : ContentDefinition<T>
    {
        public override string Key { get; }

        public override string Path { get; }

        public RuntimeContentDefinition(string key, string path)
        {
            this.Key = key;
            this.Path = path;
        }
    }
}
