using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    [Service<IGlobalComponent>(ServiceLifetime.Scoped, true)]
    public abstract class GlobalComponent : IGlobalComponent
    {
        public bool Ready { get; private set; } = false;

        void IGlobalComponent.Initialize(IGlobalComponent[] components)
        {
            if (this.Ready)
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
