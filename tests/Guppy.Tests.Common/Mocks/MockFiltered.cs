using Guppy.Core.Common;

namespace Guppy.Tests.Common.Mocks
{
    public class MockFiltered<T> : List<T>, IFiltered<T>
        where T : class
    {
        public MockFiltered()
        {
        }

        public MockFiltered(IEnumerable<T> collection) : base(collection)
        {
        }

        public Lazy<IFiltered<T>> ToLazy()
        {
            return new Lazy<IFiltered<T>>(() => this);
        }
    }
}
