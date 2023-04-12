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
        public readonly string[] Names;
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

        internal Selector(Type type, string[] names)
        {
            ThrowIf.Type.IsNotAssignableFrom<Element>(type);
            
            this.Type = type;
            this.Names = names;
        }

        private bool Check(Selector? selector, out int strength)
        {
            if (selector is null)
            {
                strength = 0;
                return false;
            }

            if (!this.Type.IsAssignableFrom(selector.Type))
            {
                strength = 0;
                return false;
            }

            strength = 1;

            foreach (string name in this.Names)
            {
                if (selector.Names.Contains(name))
                {
                    strength++;
                }
            }

            return true;
        }

        public int Match(Selector? selector)
        {
            int result;
            Selector? query = this;

            if(!query.Check(selector, out result))
            {
                return 0;
            }

            while(true)
            {
                if(query is null)
                {
                    return result;
                }

                if (selector is null)
                {
                    return 0;
                }

                if (query.Check(selector, out int strength))
                {
                    result += strength;
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
