using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public class Property
    {
        public static readonly Property TextColor = Create<Color>(true, ElementState.None, ElementState.Hovered, ElementState.Focused, ElementState.Focused);

        public static readonly Property BackgroundColor = Create<Color>(false, ElementState.None, ElementState.Hovered, ElementState.Focused, ElementState.Focused);
        public static readonly Property Test = Create<string>(false);

        public readonly Type Type;
        public readonly bool Inherit;
        public readonly ElementState State;

        public virtual Property this[ElementState state] => throw new InvalidOperationException();

        private Property(Type type, bool inherit, ElementState state)
        {
            this.Type = type;
            this.Inherit = inherit;
            this.State = state;
        }

        private class StateProperty : Property
        {
            private Property[] _properties;

            public StateProperty(Type type, bool inherit, IEnumerable<ElementState> states) : base(type, inherit, ElementState.Any)
            {
                _properties = states.Distinct()
                    .Order()
                    .Select(x =>
                    {
                        if (x == ElementState.Any)
                        {
                            throw new NotImplementedException();
                        }

                        return new Property(type, inherit, x);
                    })
                    .Concat(this.Yield())
                    .ToArray();
            }

            public override Property this[ElementState state]
            {
                get
                {
                    for(int i=0; i< _properties.Length; i++)
                    {
                        if ((_properties[i].State & state) != 0)
                        {
                            return _properties[i];
                        }
                    }

                    // This should never happen because there should always be the source
                    // parameter, with a state of "Any", which would capture all inputs.
                    throw new UnreachableException();
                }
            }
        }

        public static Property Create<T>(
            bool inherit, 
            params ElementState[] states)
        {
            if(states.Length == 0)
            {
                return new Property(typeof(T), inherit, ElementState.Any);
            }

            return new StateProperty(typeof(T), inherit, states);
        }
    }
}
