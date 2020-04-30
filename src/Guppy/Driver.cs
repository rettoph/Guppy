using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Driver : Frameable
    {
        protected Object driven { get; private set; }

        internal virtual void SetDriven(Object driven)
        {
            this.driven = driven;
        }
    }
    public class Driver<T> : Driver
        where T : Driven
    {
        protected new T driven { get; private set; }

        internal override void SetDriven(Object driven)
        {
            ExceptionHelper.ValidateAssignableFrom<T>(driven.GetType());

            this.driven = driven as T;
        }
    }
}
