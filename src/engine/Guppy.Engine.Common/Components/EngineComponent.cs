namespace Guppy.Engine.Common.Components
{
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
