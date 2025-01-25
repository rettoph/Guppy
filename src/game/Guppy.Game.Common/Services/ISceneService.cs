using Guppy.Core.Common;

namespace Guppy.Game.Common.Services
{
    public interface ISceneService
    {
        event OnEventDelegate<ISceneService, IScene>? OnSceneCreated;
        event OnEventDelegate<ISceneService, IScene>? OnSceneDestroyed;

        IScene Create(Type guppyType, Action<IGuppyScopeBuilder>? builder = null);

        T Create<T>(Action<IGuppyScopeBuilder>? builder = null)
            where T : class, IScene;

        /// <summary>
        /// Remove, dispose, and cleanup the given IScene instance
        /// </summary>
        /// <param name="guppy"></param>
        void Destroy(IScene guppy);

        IEnumerable<IScene> GetAll();
    }
}