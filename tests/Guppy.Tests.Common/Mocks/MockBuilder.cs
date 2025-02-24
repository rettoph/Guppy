namespace Guppy.Tests.Common.Mocks
{
    public abstract class MockBuilder<T>
    {
        private T? _instance;
        protected T GetInstance()
        {
            if (this._instance is null)
            {
                this.PreBuild();
                this._instance = this.Build();
                this.PostBuild();
            }

            return this._instance;
        }

        protected virtual void PreBuild()
        {
            //
        }
        protected abstract T Build();

        protected virtual void PostBuild()
        {
            //
        }
    }
}
