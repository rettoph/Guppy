using Guppy.Core.Common.Builders;

namespace Guppy.Game.Common.Services
{
    public interface ISceneService
    {
        IScene Create(Type guppyType, Action<IGuppyScopeBuilder>? builder = null);
        IScene CreateAndInitialize(Type guppyType, Action<IGuppyScopeBuilder>? builder = null)
        {
            IScene scene = this.Create(guppyType, builder);
            scene.Initialize();
            return scene;
        }

        T Create<T>(Action<IGuppyScopeBuilder>? builder = null)
            where T : class, IScene;
        T CreateAndInitialize<T>(Action<IGuppyScopeBuilder>? builder = null)
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
        void Destroy(IScene guppy);

        IEnumerable<IScene> GetAll();
    }
}