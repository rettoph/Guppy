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

        public bool Match(Selector selector)
        {
            if (!this.Type.IsAssignableFrom(selector.Type))
            {
                return false;
            }

            foreach (string name in this.Names)
            {
                if (!selector.Names.Contains(name))
                {
                    return false;
                }
            }

            return true;
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
