using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    public interface ICommandService
    {
        void Invoke(string input);
        void Publish(IMessage command);
        void Subscribe(IBus bus);
        void Unsubscribe(IBus bus);
    }
}
