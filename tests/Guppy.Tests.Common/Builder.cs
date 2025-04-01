namespace Guppy.Tests.Common
{
    public abstract class Builder<T> : IBuilder<T>
        where T : class
    {
        private T? _instance;
        public T Object => this._instance ??= this.GetInstance();

        object IBuilder.Object => this.Object;

        private T GetInstance()
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
