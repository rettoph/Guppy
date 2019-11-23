using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Network.Utilitites.Delegaters
{
    /// <summary>
    /// A simple delegater that can recieve string inputs & will
    /// automatically hash them into a more effecient placeholder 
    /// value.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    public class HashedDelegater<TArg> : CustomDelegater<UInt32, TArg>
    {
        #region Helper Methods
        public void TryAdd(String key, CustomDelegate<TArg> d)
        {
            this.Add<TArg>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(key)), d);
        }

        public void TryRemove(String key, CustomDelegate<TArg> d)
        {
            this.Remove<TArg>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(key)), d);
        }
        public void TryInvoke(Object sender, String key, TArg arg)
        {
            this.Invoke<TArg>(sender, xxHash.CalculateHash(Encoding.UTF8.GetBytes(key)), arg);
        }
        #endregion
    }
}
