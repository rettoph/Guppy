namespace Guppy.Tests.Common
{
    public abstract class Builder<T> : IBuilder<T>
        where T : notnull
    {
        private T? _instance;

        public T Object => this.GetInstance();

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
