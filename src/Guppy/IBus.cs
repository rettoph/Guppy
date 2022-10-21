﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBus<T> : IBroker<T>
        where T : notnull, IMessage
    {
        Guid Id { get; }

        void Flush();
    }

    public interface IBus : IBus<IMessage>
    {

    }
}
