using Guppy.Attributes;
using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    [Service<IGlobalComponent>(ServiceLifetime.Singleton, true)]
    public abstract class GlobalComponent : IGlobalComponent
    {
        public bool Ready { get; private set; } = false;

        void IGlobalComponent.Initialize(IGlobalComponent[] components)
        {
            if(this.Ready)
            {
                return;
            }

            this.Initialize(components);

            this.Ready = true;
        }

        protected virtual void Initialize(IGlobalComponent[] components)
        {
        }
    }
}
