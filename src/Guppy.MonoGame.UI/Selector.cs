using Guppy.MonoGame.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public sealed class Selector
    {
        private static readonly HashSet<Selector> _cycleCheck = new HashSet<Selector>();

        private int _specificity;
        private Selector? _parent;

        public readonly Type Type;
        public readonly string[] Names;
        public Selector? Parent
        {
            get => _parent;
            internal set
            {
                if(value is not null)
                {
                    _cycleCheck.Clear();
                    _cycleCheck.Add(this);
                    Selector? selector = value;

                    while(selector is not null)
                    {
                        if(!_cycleCheck.Add(selector))
                        {
                            _parent = null;
                            return;
                        }

                        selector = selector.Parent;
                    }
                }

                _parent = value;
            }
        }
        public int Specificity
        {
            get
            {
                if(_specificity == 0)
                {
                    Type type = this.Type;

                    while (type != typeof(object) && !type.IsInterface)
                    {
                        _specificity++;
                        type = type.BaseType ?? typeof(object);
                    }


                    _specificity += this.Names.Length;
                }

                return _specificity + (this.Parent?.Specificity ?? 0);
            }
        }

        internal Selector(Type type, string[] names)
        {
            this.Type = type;
            this.Names = names;
        }

        public bool Includes(Selector selector)
        {
            if(this == selector)
            {
                return true;
            }

            if(!this.Engulfs(selector))
            {
                return false;
            }

            Selector? parent = this.Parent;
            while(parent is not null && selector.Parent is not null)
            {
                if(parent.Engulfs(selector.Parent))
                {
                    parent = parent.Parent;
                }

                selector = selector.Parent;
            }

            return parent is null;
        }

        private bool Engulfs(Selector selector)
        {
            if(!this.Type.IsAssignableFrom(selector.Type))
            {
                return false;
            }

            foreach(string name in this.Names)
            {
                if (!selector.Names.Contains(name))
                {
                    return false;
                }
            }

            return true;
        }

        public Selector Child(params string[] names)
        {
            return this.Child<IElement>(names);
        }
        public Selector Child<T>(params string[] names)
            where T : IElement
        {
            return new Selector(typeof(T), names)
            {
                Parent = this
            };
        }

        public static Selector Create(params string[] names)
        {
            return Create<IElement>(names);
        }
        public static Selector Create<T>(params string[] names)
            where T : IElement
        {
            return new Selector(typeof(T), names);
        }

        public override bool Equals(object? obj)
        {
            return obj is Selector selector &&
                   EqualityComparer<Type>.Default.Equals(Type, selector.Type) &&
                   EqualityComparer<string[]>.Default.Equals(Names, selector.Names) &&
                   EqualityComparer<Selector?>.Default.Equals(Parent, selector.Parent);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Names, Parent);
        }

        public static bool operator ==(Selector left, Selector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Selector left, Selector right)
        {
            return !left.Equals(right);
        }
    }
}
