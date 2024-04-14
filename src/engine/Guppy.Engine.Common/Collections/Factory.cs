namespace Guppy.Engine.Common.Collections
{
    public class Factory<T> : Pool<T>
        where T : class
    {
        private Func<T> _method;

        public Factory(Func<T> method, ushort maxPoolSize = 50) : base(maxPoolSize)
        {
            _method = method;
        }

        public virtual T BuildInstance()
        {
            if (!this.TryPull(out T? instance))
            {
                instance = this.Build();
            }

            return instance;
        }

        protected virtual T Build()
        {
            return _method();
        }
    }
}
