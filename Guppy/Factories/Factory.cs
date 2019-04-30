using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public abstract class Factory<T>
        where T : class
    {
        protected Type targetType;

        protected Factory()
        {
            this.targetType = typeof(T);
        }

        // Create a new instance of the current factory object
        public abstract T Create(IServiceProvider provider);
        public abstract T CreateCustom(IServiceProvider provider, params Object[] args);
    }
}
