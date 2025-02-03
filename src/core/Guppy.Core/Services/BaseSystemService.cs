using System.Collections;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public abstract class BaseSystemService<TSystem>(IEnumerable<TSystem> systems) : ISystemService<TSystem>
        where TSystem : ISystem
    {
        private readonly List<TSystem> _systems = [.. systems];

        public IEnumerable<T> GetAll<T>()
        {
            return this._systems.OfType<T>();
        }

        public IEnumerator<TSystem> GetEnumerator()
        {
            return this._systems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
