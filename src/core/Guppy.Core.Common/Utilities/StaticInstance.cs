namespace Guppy.Core.Common.Utilities
{
    public static class StaticInstance<T>
    {
        private static bool _initialized;
        private static bool _isReadonly;
        private static T _value = default!;

        public static T Value
        {
            get
            {
                if (_initialized == false)
                {
                    throw new InvalidOperationException();
                }

                return _value;
            }
        }

        public static void Initialize(T instance, bool isReadonly)
        {
            if (_initialized)
            {
                throw new InvalidOperationException();
            }

            _isReadonly = isReadonly;
            _value = instance;
            _initialized = true;
        }

        public static void SetValue(T instance)
        {
            if (_initialized == false)
            {
                throw new InvalidOperationException();
            }

            if (_isReadonly)
            {
                throw new InvalidOperationException();
            }

            _value = instance;
        }
    }
}