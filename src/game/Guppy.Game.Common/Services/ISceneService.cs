using Guppy.Core.Common.Builders;

namespace Guppy.Game.Common.Services
{
    public interface ISceneService
    {
        public IScene Create(Type guppyType, Action<IGuppyScopeBuilder>? builder = null);
        public IScene CreateAndInitialize(Type guppyType, Action<IGuppyScopeBuilder>? builder = null)
        {
            IScene scene = this.Create(guppyType, builder);
            scene.Initialize();
            return scene;
        }

        public T Create<T>(Action<IGuppyScopeBuilder>? builder = null)
            where T : class, IScene;
        public T CreateAndInitialize<T>(Action<IGuppyScopeBuilder>? builder = null)
            where T : class, IScene
        {
            T scene = this.Create<T>(builder);
            scene.Initialize();
            return scene;
        }

        /// <summary>
        /// Remove, dispose, and cleanup the given IScene instance
        /// </summary>
        /// <param name="guppy"></param>
        public void Destroy(IScene guppy);

        public IEnumerable<IScene> GetAll();
    }
}