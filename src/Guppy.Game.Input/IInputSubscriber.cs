using Guppy.Common;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input
{
    public interface IInputSubscriber<TInput> : IBaseSubscriber<IInput, TInput>
        where TInput : IInput
    {
    }
}
