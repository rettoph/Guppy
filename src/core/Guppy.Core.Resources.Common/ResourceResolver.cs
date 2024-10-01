namespace Guppy.Core.Resources.Common
{
    public abstract class ResourceResolver
    {
        internal ResourceResolver()
        {

        }
    }

    public sealed class ResourceResolver<T>(Func<T> resolver) : ResourceResolver
    {
        private readonly Func<T> _getter = resolver;
        private T _value = default!;
        private bool _resolved = false;

        public T Value
        {
            get
            {
                lock (this)
                {
                    if (_resolved)
                    {
                        return _value;
                    }

                    _value = _getter();
                    _resolved = true;
                    return _value;
                }

            }
        }

        public void Reset()
        {
            lock (this)
            {
                _resolved = false;
            }
        }
    }
}
