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
        private Selector? _parent;

        public readonly Type Type;
        public readonly List<string> Names;
        public Selector? Parent
        {
            get => _parent;
            set
            {
                if(IsCircularReference(this, value))
                {
                    throw new InvalidOperationException();
                }

                _parent = value;
            }
        }

        internal Selector(Type type, IEnumerable<string> names)
        {
            ThrowIf.Type.IsNotAssignableFrom<Element>(type);
            
            this.Type = type;
            this.Names = new List<string>(names);
        }

        private bool Check(Selector? selector)
        {
            if (selector is null)
            {
                return false;
            }

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

        public bool Match(Selector? selector)
        {
            Selector? query = this;

            if(!query.Check(selector))
            {
                return false;
            }

            while(true)
            {
                if(query is null)
                {
                    return true;
                }

                if (selector is null)
                {
                    return false;
                }

                if (query.Check(selector))
                {
                    query = query.Parent;
                }

                // Because we guarantee there are no circular references when
                // Defining this.Parent, we can be certain selector.Parent will
                // eventually result in a null value, breaking the loop
                selector = selector.Parent;
            }
        }

        public Selector Child<T>(params string[] names)
            where T : Element
        {
            Selector child = Selector.Create<T>(names);
            child.Parent = this;

            return child;
        }

        private static bool IsCircularReference(Selector child, Selector? parent)
        {
            while(parent is not null)
            {
                if (parent == child)
                {
                    return true;
                }

                parent = parent.Parent;
            }
            
            return false;
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
