using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input
{
    public interface IInputSubscriber : IBaseSubscriber<IInput>
    {

    }
    public interface IInputSubscriber<TInput> : IInputSubscriber, IBaseSubscriber<IInput, TInput>
        where TInput : IInput
    {
    }
}
