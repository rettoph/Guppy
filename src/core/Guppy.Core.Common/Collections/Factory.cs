namespace Guppy.Core.Common.Collections
{
    public class Factory<T>(Func<T> method, ushort maxPoolSize = 50) : Pool<T>(maxPoolSize)
        where T : class
    {
        private readonly Func<T> _method = method;

        public virtual T GetOrCreate()
        {
            if (!this.TryPull(out T? instance))
            {
                instance = this.Create();
            }

            return instance;
        }

        protected virtual T Create()
        {
            return this._method();
        }
    }
}