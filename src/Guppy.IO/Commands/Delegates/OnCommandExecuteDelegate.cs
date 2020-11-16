using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands.Delegates
{
    /// <summary>
    /// When a command gets excecuted
    /// </summary>
    /// <param name="sender">The Command currently being invoked.</param>
    /// <param name="source">The source command that srtarted the invocation.</param>
    /// <param name="input">The specific CommandInput instance recieved.</param>
    public delegate CommandResponse OnCommandExecuteDelegate(ICommand sender, CommandInput input);

    public static class OnCommandExecuteDelegateExtensions
    {
        public static CommandResponse TryInvoke(this OnCommandExecuteDelegate del, ICommand sender, CommandInput input)
        {
            try
            {
                return del.Invoke(sender, input);
            }
            catch (Exception e)
            {
                return CommandResponse.Error(exception: e);
            }
        }

        public static IEnumerable<CommandResponse> LazyInvoke(this OnCommandExecuteDelegate del, ICommand sender, CommandInput input)
        {
            if (del == default)
                return Enumerable.Empty<CommandResponse>();
            else
                return del.LazyInvokeIterator(sender, input);
        }

        private static IEnumerable<CommandResponse> LazyInvokeIterator(this OnCommandExecuteDelegate del, ICommand sender, CommandInput input)
        {
            foreach (OnCommandExecuteDelegate commandDelegate in del.GetInvocationList())
                yield return commandDelegate.TryInvoke(sender, input);
        }
    }
}
