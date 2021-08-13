using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    public delegate void OnChangedEventDelegate<TSender, TValue>(TSender sender, TValue old, TValue value);

    public static class OnChangedEventDelegateExtensions
    {
        /// <summary>
        /// If <paramref name="changed"/> is true, update the
        /// <paramref name="old"/> reference to <paramref name="value"/>
        /// and invoke the <paramref name="deltaDelegate"/> event.
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="deltaDelegate"></param>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        public static Boolean InvokeIf<TSender, TValue>(
            this OnChangedEventDelegate<TSender, TValue> deltaDelegate, 
            Boolean changed, 
            TSender sender, 
            ref TValue old, 
            TValue value)
        {
            if (!changed) // Do nothing
                return false;
            else if(deltaDelegate == default) // Just update the reference
                old = value;
            else // Invoke event and update reference
                deltaDelegate.Invoke(sender, old, old = value);
            return true;
        }
    }
}
