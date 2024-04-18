namespace Guppy.Game.Common.Components
{
    public abstract class SceneComponent : ISceneComponent
    {
        public bool Ready { get; private set; }

        void ISceneComponent.Initialize()
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
