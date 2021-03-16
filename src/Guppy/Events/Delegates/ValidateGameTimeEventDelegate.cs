using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    /// <summary>
    /// Multicast Predicate Delegate that will return false
    /// if anything invoked returns false.
    /// 
    /// Try using the Validate() extension method.
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    /// <typeparam name="TArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="gameTime"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate Boolean ValidateGameTimeEventDelegate<TSender, TArgs>(TSender sender, GameTime gameTime, TArgs args);

    public static class ValidateEventDelegateExtensions
    {
        public static Boolean Validate<TSender, TArgs>(this ValidateGameTimeEventDelegate<TSender, TArgs> validator, TSender sender, GameTime gameTime, TArgs args, Boolean ifNull)
        {
            if (validator == default)
                return ifNull;
            else
                foreach (ValidateGameTimeEventDelegate<TSender, TArgs> v in validator.GetInvocationList())
                    if (!v(sender, gameTime, args))
                        return false;

            return true;
        }

        public static ValidateGameTimeEventDelegate<TSender, TArgs> ToValidateGameTimeEventDelegate<TSender, TArgs>(this Func<TSender, GameTime, TArgs, Boolean> func)
            => (s, gt, a) => func(s, gt, a);
    }
}
