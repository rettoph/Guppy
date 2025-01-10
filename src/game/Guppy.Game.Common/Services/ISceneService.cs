using Autofac;

namespace Guppy.Game.Common.Services
{
    public interface ISceneService
    {
        event OnEventDelegate<ISceneService, IScene>? OnSceneCreated;
        event OnEventDelegate<ISceneService, IScene>? OnSceneDestroyed;

        IScene Create(Type guppyType, Action<ISceneConfiguration>? configuration = null, Func<ILifetimeScope, object>? factory = null);

        T Create<T>(Action<ISceneConfiguration>? configuration = null, Func<ILifetimeScope, T>? factory = null)
            where T : class, IScene;

        /// <summary>
        /// Remove, dispose, and cleanup the given IScene instance
        /// </summary>
        /// <param name="guppy"></param>
        void Destroy(IScene guppy);

        IEnumerable<IScene> GetAll();
    }
}