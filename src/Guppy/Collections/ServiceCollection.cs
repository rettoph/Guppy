using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.Collections
{
    public class ServiceCollection<TService> : ProtectedServiceCollection<TService>
        where TService : IService
    {
        #region Helper Methods
        public void TryAdd(TService item)
            => this.Add(item);

        public Boolean TryRemove(TService item)
            => this.Remove(item);

        public void TryClear()
            => this.Clear();
        #endregion
    }
}
