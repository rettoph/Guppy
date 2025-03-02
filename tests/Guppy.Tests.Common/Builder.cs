using MoqProxy;

namespace Guppy.Tests.Common
{
    public abstract class Builder<T> : IBuilder<T>
        where T : class
    {
        private MockProxy<T>? _proxy;

        public T Object => this.Proxy.Object;

        object IBuilder.Object => this.Object;

        public MockProxy<T> Proxy => this._proxy ??= this.GetProxy();

        private MockProxy<T> GetProxy()
        {
            if (this._proxy is null)
            {
                this.PreBuild();
                this._proxy = new MockProxy<T>(this.Build());
                this.PostBuild();
            }

            return this._proxy;
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
