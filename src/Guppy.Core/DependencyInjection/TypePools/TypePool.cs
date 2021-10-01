using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.TypePools
{
    /// <summary>
    /// Represents a pool of services.
    /// </summary>
    public sealed class TypePool : Pool<Object>
    {
        private Type _type;

        public TypePool(Type type, ref UInt16 maxPoolSize) : base(ref maxPoolSize)
        {
            _type = type;
        }

        public override Boolean TryReturn(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(_type, instance.GetType());

            return base.TryReturn(instance);
        }
    }
}
