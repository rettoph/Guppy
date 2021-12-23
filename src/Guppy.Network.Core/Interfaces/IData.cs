using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Builders;

namespace Guppy.Network.Interfaces
{
    public interface IData : IMessage
    {
        /// <summary>
        /// Simple method invoked immidiately after the message gets processed once.
        /// Can be used to clean any unnecessary resources once the message can
        /// be killed.
        /// </summary>
        void Clean();
    }
}
