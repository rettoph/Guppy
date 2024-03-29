﻿using Guppy.Messaging;

namespace Guppy.Commands
{
    public interface ICommandSubscriber<T> : IBaseSubscriber<ICommand, T>
        where T : ICommand
    {
    }
}
