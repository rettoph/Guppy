namespace Guppy.Tests.Common.Mocks
{
    public abstract class BaseMockerBuilder<T>
    {
        private T? _instance;

        public T GetInstance()
        {
            return this._instance ??= this.Build();
        }

        protected abstract T Build();
    }
}
