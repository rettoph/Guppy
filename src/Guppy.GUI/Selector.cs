using Guppy.Common;
using Guppy.GUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public sealed class Selector
    {
        public readonly Type Type;
        public readonly string[] Names;

        internal Selector(Type type, string[] names)
        {
            ThrowIf.Type.IsNotAssignableFrom<Element>(type);
            
            this.Type = type;
            this.Names = names;
        }

        public int Match(Selector selector)
        {
            if (!this.Type.IsAssignableFrom(selector.Type))
            {
                return 0;
            }

            int match = 1;

            foreach (string name in selector.Names)
            {
                if (!this.Names.Contains(name))
                {
                    match++;
                }
            }

            return match;
        }

        public static Selector Create(params string[] names)
        {
            return Create<Element>(names);
        }
        public static Selector Create<T>(params string[] names)
            where T : Element
        {
            return new Selector(typeof(T), names);
        }
    }
}
