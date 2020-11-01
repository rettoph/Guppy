using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class GuppyServiceProvider
    {
        #region GetService Methods
        /// <summary>
        /// Return a new instance of a service 
        /// based on the recieved service id value
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public T GetService<T>(
            UInt32 id,
            Action<T, GuppyServiceProvider, ServiceContext> setup = null
        )
            => (T)this.GetService(id, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new instance of a service 
        /// based on the recieved service name value
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public T GetService<T>(
            String name,
            Action<T, GuppyServiceProvider, ServiceContext> setup = null
        )
            => (T)this.GetService(name, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved type
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public T GetService<T>(
            Type type,
            Action<T, GuppyServiceProvider, ServiceContext> setup = null
        )
            => (T)this.GetService(type, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved generic type
        /// and cast into type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public T GetService<T>(
            Action<T, GuppyServiceProvider, ServiceContext> setup = null
        )
            => (T)this.GetService(typeof(T), (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Automaitcally set the out value via the intenral GetService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public void Service<T>(out T service, Action<T, GuppyServiceProvider, ServiceContext> setup = null)
            => service = this.GetService<T>(setup);

        /// <summary>
        /// Automaitcally set the out value via the intenral GetService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public void Service<T>(out T service, String name, Action<T, GuppyServiceProvider, ServiceContext> setup = null)
            => service = this.GetService<T>(name, setup);
        #endregion
    }
}
