namespace Guppy.Core.Resources.Common
{
    public abstract class ResourceResolver
    {
        internal ResourceResolver()
        {

        }
    }

    public sealed class ResourceResolver<T> : ResourceResolver
    {
        private readonly Func<T> _getter;
        private T _value;
        private bool _resolved;

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

        public ResourceResolver(Func<T> resolver)
        {
            _value = default!;
            _getter = resolver;
            _resolved = false;
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
