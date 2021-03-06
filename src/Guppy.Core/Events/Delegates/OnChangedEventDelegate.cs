﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    public delegate void OnChangedEventDelegate<TSender, TValue>(TSender sender, TValue old, TValue value);

    public static class OnChangedEventDelegateExtensions
    {
        /// <summary>
        /// Attempt to update the reference value & then
        /// invoke the event if possible.
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="deltaDelegate"></param>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        public static Boolean InvokeIf<TSender, TValue>(
            this OnChangedEventDelegate<TSender, TValue> deltaDelegate, 
            Boolean valid, 
            TSender sender, 
            ref TValue old, 
            TValue value)
        {
            if (!valid)
                return false;
            else if(deltaDelegate == default(OnChangedEventDelegate<TSender, TValue>))
                old = value;
            else
                deltaDelegate.Invoke(sender, old, old = value);
            return true;
        }
    }
}
