using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    [Service(ServiceLifetime.Singleton, true)]
    public abstract class EngineComponent : IEngineComponent
    {
        public bool Ready { get; private set; } = false;

        void IEngineComponent.Initialize()
        {
            if (this.Ready)
            {
                return;
            }

            this.Initialize();

            this.Ready = true;
        }

        protected virtual void Initialize()
        {
        }
    }
}
