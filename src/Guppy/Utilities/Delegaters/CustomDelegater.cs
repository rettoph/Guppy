using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Delegaters
{
    /// <summary>
    /// Extremely basic delegater without dynamic
    /// argument types. There is no need for custom
    /// registration as seen in the event handler
    /// delegater.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TArg"></typeparam>
    public class CustomDelegater<TKey, TArg> : IDisposable
    {
        #region Delegates
        public delegate void CustomDelegate<T>(Object sender, T arg)
            where T : TArg;
        #endregion

        #region Protected Fields
        protected Dictionary<TKey, Delegate> delegates;
        #endregion

        #region Constructor
        public CustomDelegater()
        {
            this.delegates = new Dictionary<TKey, Delegate>();
        }
        #endregion

        #region Add Methods
        public void TryAdd(TKey key, CustomDelegate<TArg> d)
        {
            this.Add<TArg>(key, d);
        }
        protected virtual void Add<T>(TKey key, CustomDelegate<T> d)
            where T : TArg
        {
            if (this.delegates.ContainsKey(key))
                this.delegates[key] = Delegate.Combine(this.delegates[key], d);
            else
                this.delegates[key] = d;
        }
        #endregion

        #region Remove Methods
        public void TryRemove(TKey key, CustomDelegate<TArg> d)
        {
            this.Remove<TArg>(key, d);
        }
        protected virtual void Remove<T>(TKey key, CustomDelegate<T> d)
            where T : TArg
        {
            if (this.delegates.ContainsKey(key))
                this.delegates[key] = Delegate.Remove(this.delegates[key], d);
        }
        #endregion

        #region Invoke Methods
        public void TryInvoke(Object sender, TKey key, TArg arg)
        {
            this.Invoke<TArg>(sender, key, arg);
        }
        protected virtual void Invoke<T>(Object sender, TKey key, T arg)
            where T : TArg
        {
            (this.delegates[key] as CustomDelegate<T>)?.Invoke(sender, arg);
        }
        #endregion

        public virtual void Dispose()
        {
            this.delegates.Clear();
        }
    }
}
