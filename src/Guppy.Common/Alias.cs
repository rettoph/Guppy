using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    [DebuggerDisplay("Alias: {Type.Name}, Implementation: {ImplementationType.Name}")]
    public sealed class Alias
    {
        public readonly Type Type;
        public readonly Type ImplementationType;

        public Alias(Type type, Type implementationType)
        {
            ThrowIf.Type.IsNotAssignableFrom(type, implementationType);

            this.Type = type;
            this.ImplementationType = implementationType;
        }

        public static Alias Create<T, TImplementation>()
        {
            return new Alias(typeof(T), typeof(TImplementation));
        }

        public static IEnumerable<Alias> FromMany<TImplementation>(params Type[] aliases)
        {
            foreach(Type alias in aliases)
            {
                yield return new Alias(alias, typeof(TImplementation));
            }
        }

        public static IEnumerable<Alias> ForMany<TAlias>(params Type[] implementations)
        {
            foreach (Type implementation in implementations)
            {
                yield return new Alias(typeof(TAlias), implementation);
            }
        }
    }
}
